﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Automatic9045.AtsEx.Plugins.Scripting;
using Automatic9045.AtsEx.PluginHost.ClassWrappers;
using Automatic9045.AtsEx.PluginHost.ExtendedBeacons;
using Automatic9045.AtsEx.PluginHost.Plugins;

namespace Automatic9045.AtsEx.ExtendedBeacons
{
    internal class InitializerBeacon : Beacon
    {
        private bool IsFirstTime = true;

        public InitializerBeacon(BveHacker bveHacker, IReadOnlyDictionary<PluginType, PluginVariableCollection> pluginVariables,
            string name, RepeatedStructure definedStructure, ObservingTargetTrack observingTargetTrack, ObservingTargetTrain observingTargetTrain,
            IPluginScript<ExtendedBeaconGlobalsBase<PassedEventArgs>> script)
            : base(bveHacker, pluginVariables, name, definedStructure, observingTargetTrack, observingTargetTrain, script)
        {
        }

        internal override void Tick(double currentLocation)
        {
            if (!IsFirstTime) return;

            NotifyPassed(Direction.Forward);
            IsFirstTime = false;
        }
    }
}
