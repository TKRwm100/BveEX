﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Plugins;
using AtsEx.Plugins.Extensions;
using AtsEx.Plugins.Scripting;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;

namespace AtsEx
{
    internal partial class AtsEx
    {
        private class ExtensionLoader
        {
            private readonly BveHacker BveHacker;

            public ExtensionLoader(BveHacker bveHacker)
            {
                BveHacker = bveHacker;
            }

            public ExtensionSet Load()
            {
                ExtensionSet extensions = new ExtensionSet();
                PluginLoader pluginLoader = new PluginLoader(null, BveHacker, extensions, null);

                string extensionsDirectory = Path.Combine(Path.GetDirectoryName(App.Instance.AtsExAssembly.Location), "Extensions");
                Directory.CreateDirectory(extensionsDirectory);

                string pluginUsingPath = Path.Combine(extensionsDirectory, "PluginUsing.xml");
                PluginSourceSet fromPluginUsing = File.Exists(pluginUsingPath)
                    ? PluginSourceSet.FromPluginUsing(PluginType.Extension, true, pluginUsingPath) : PluginSourceSet.Empty(PluginType.Extension);
                PluginSourceSet fromDirectory = PluginSourceSet.FromDirectory(null, PluginType.Extension, true, extensionsDirectory);

                Dictionary<string, PluginBase> loadedExtensions = pluginLoader.Load(fromPluginUsing.Concat(null, fromDirectory));

                extensions.SetExtensions(loadedExtensions.Values);
                return extensions;
            }
        }
    }
}
