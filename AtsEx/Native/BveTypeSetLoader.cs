﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;
using UnembeddedResources;

using AtsEx.PluginHost;

namespace AtsEx.Native
{
    internal class BveTypeSetLoader
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BveTypeSetLoader>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> IllegalSlimDXDetected { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BveTypeSetLoader()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public event EventHandler<ProfileForDifferentVersionBveLoadedEventArgs> ProfileForDifferentVersionBveLoaded;

        public BveTypeSetLoader()
        {
        }

        public BveTypeSet Load()
        {
            try
            {
                Version bveVersion = App.Instance.BveVersion;
                BveTypeSet bveTypes = BveTypeSet.LoadAsync(
                    App.Instance.BveAssembly, bveVersion, true,
                    profileVersion => ProfileForDifferentVersionBveLoaded?.Invoke(this, new ProfileForDifferentVersionBveLoadedEventArgs(bveVersion, profileVersion)))
                    .Result;

                return bveTypes;
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    CheckSlimDX();
                }

                ExceptionResolver exceptionResolver = new ExceptionResolver();
                string senderName = Path.GetFileName(typeof(BveTypeSet).Assembly.Location);
                exceptionResolver.Resolve(senderName, ex);
                throw;
            }


            void CheckSlimDX()
            {
                IEnumerable<Assembly> slimDXAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(asm => asm.GetName().Name == "SlimDX");

                if (slimDXAssemblies.Count() > 1)
                {
                    string locationText = string.Join("\n", slimDXAssemblies.Select(assembly => "・" + assembly.Location));
                    MessageBox.Show(string.Format(Resources.Value.IllegalSlimDXDetected.Value, locationText), App.Instance.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public class ProfileForDifferentVersionBveLoadedEventArgs : EventArgs
        {
            public Version BveVersion { get; }
            public Version ProfileVersion { get; }

            public ProfileForDifferentVersionBveLoadedEventArgs(Version bveVersion, Version profileVersion)
            {
                BveVersion = bveVersion;
                ProfileVersion = profileVersion;
            }
        }

        public class IllegalSlimDXDetectedEventArgs : EventArgs
        {
            public IEnumerable<string> AssemblyLocations { get; }

            public IllegalSlimDXDetectedEventArgs(IEnumerable<string> assemblyLocations)
            {
                AssemblyLocations = assemblyLocations;
            }
        }
    }
}
