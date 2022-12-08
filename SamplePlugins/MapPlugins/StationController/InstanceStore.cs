﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Samples.MapPlugins.StationController
{
    internal sealed class InstanceStore
    {
        public static InstanceStore Instance { get; private set; } = null;
        public static bool IsInitialized => !(Instance is null);

        public static void Initialize(INative native, IExtensionSet extensions, IBveHacker bveHacker)
        {
            Instance = new InstanceStore(native, extensions, bveHacker);
        }


        public INative Native { get; }
        public IExtensionSet Extensions { get; }
        public IBveHacker BveHacker { get; }

        private InstanceStore(INative native, IExtensionSet extensions, IBveHacker bveHacker)
        {
            Native = native;
            Extensions = extensions;
            BveHacker = bveHacker;
        }
    }
}
