using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using HarmonyLib;

using SlimDX.Direct3D9;

using AtsEx.PluginHost.ClassWrappers;

#pragma warning disable CS1591 // ���J����Ă���^�܂��̓����o�[�� XML �R�����g������܂���
namespace Zbx1425.DXDynamicTexture
{
    public static class TextureManager
    {
        private static bool IsInitialized = false;

        internal static Harmony Harmony;
        internal static Dictionary<string, TextureHandle> Handles = new Dictionary<string, TextureHandle>();

        internal static Device DXDevice;

        public static void Initialize()
        {
            if (IsInitialized) throw new InvalidOperationException("DXDynamicTexture has been already initialized.");
            IsInitialized = true;

            Harmony = new Harmony("cn.zbx1425.dxdynamictexture");
            Harmony.Patch(typeof(Texture).GetMethods()
                .Where(m => m.Name == nameof(Texture.FromFile) && m.GetParameters().Length == 11)
                .FirstOrDefault(),
                null, new HarmonyMethod(typeof(TextureManager), nameof(FromFilePostfix))
            );

            TouchManager.Initialize();
        }

#pragma warning disable IDE1006 // �����X�^�C��
        private static void FromFilePostfix(ref Texture __result, Device device, string fileName)
#pragma warning restore IDE1006 // �����X�^�C��
        {
            var normalizedName = fileName.Replace('\\', '/').ToLowerInvariant();
            foreach (var item in Handles)
            {
                if (normalizedName.EndsWith(item.Key))
                {
                    __result = Handles[item.Key].GetOrCreate(device);
                }
            }
            DXDevice = device;
        }

        public static TextureHandle Register(string fileNameEnding, int width, int height)
        {
            if (fileNameEnding.Trim().Length == 0) throw new ArgumentException("Must not be empty.", nameof(fileNameEnding));
            if (!IsPowerOfTwo(width)) throw new ArgumentException("Must be a integral power of 2.", nameof(width));
            if (!IsPowerOfTwo(height)) throw new ArgumentException("Must be a integral power of 2.", nameof(height));

            fileNameEnding = fileNameEnding.ToLowerInvariant().Replace('\\', '/');
            if (Handles.ContainsKey(fileNameEnding)) return Handles[fileNameEnding];
            var result = new TextureHandle(width, height);
            Handles.Add(fileNameEnding, result);
            return result;
        }

        public static TextureHandle Register(this Texture texture)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));

            var result = new TextureHandle(texture);
            string guidString;
            do
            {
                guidString = Guid.NewGuid().ToString();
            } while (Handles.ContainsKey(guidString));
            Handles.Add(guidString, result);
            return result;
        }

        public static TextureHandle Register(this Model model, string textureFileName)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (textureFileName == null) throw new ArgumentNullException(nameof(textureFileName));

            ExtendedMaterial[] materials = model.Mesh.GetMaterials();
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].TextureFileName == textureFileName)
                {
                    return Register(model.Materials[i].Texture);
                }
            }

            throw new FileNotFoundException("The texture file is not found.", textureFileName);
        }

        internal static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        internal static void Dispose()
        {
            foreach (var item in Handles)
            {
                if (item.Value != null && item.Value.IsCreated) item.Value.Dispose();
            }
        }

        public static void Clear()
        {
            Dispose();
            Handles.Clear();

            IsInitialized = false;
        }
    }
}
