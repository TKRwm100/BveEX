﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    public partial class VersionSelector
    {
        public class AsAtsPlugin : VersionSelector
        {
            public new CoreHost CoreHost { get; }

            public AsAtsPlugin(Assembly callerAssembly) : base()
            {
                CoreHost = new CoreHost(callerAssembly, BveFinder);
            }
        }
    }
}
