﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.Handles;
using AtsEx.Input;
using AtsEx.Panels;
using AtsEx.Plugins;
using AtsEx.Sound;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Panels.Native;
using AtsEx.PluginHost.Sound.Native;

namespace AtsEx
{
    internal class NativeImpl : INative
    {
        public NativeImpl(VehicleSpec vehicleSpec, VehicleConfig vehicleConfigOptions)
        {
            VehicleSpec = vehicleSpec;

            BrakeHandle brake = new BrakeHandle(vehicleSpec.BrakeNotches, vehicleSpec.AtsNotch, vehicleSpec.B67Notch, false);
            PowerHandle power = new PowerHandle(VehicleSpec.PowerNotches);
            Reverser reverser = new Reverser();
            Handles = new PluginHost.Handles.HandleSet(brake, power, reverser);

            AtsPanelValues = new AtsPanelValueSet(vehicleConfigOptions.DetectPanelValueIndexConflict);
            AtsSounds = new AtsSoundSet(vehicleConfigOptions.DetectSoundIndexConflict);
        }

        public void InvokeStarted(BrakePosition defaultBrakePosition)
        {
            StartedEventArgs e = new StartedEventArgs(defaultBrakePosition);
            Started?.Invoke(e);
        }

        public void InvokeDoorOpened(DoorEventArgs args) => DoorOpened?.Invoke(args);

        public void InvokeDoorClosed(DoorEventArgs args) => DoorClosed?.Invoke(args);

        public void InvokeBeaconPassed(BeaconPassedEventArgs args) => BeaconPassed?.Invoke(args);

        public PluginHost.Handles.HandleSet Handles { get; }

        public IAtsPanelValueSet AtsPanelValues { get; }

        public INativeKeySet NativeKeys { get; } = new NativeKeySet();
        public IAtsSoundSet AtsSounds { get; }

        public VehicleSpec VehicleSpec { get; }

        public VehicleState VehicleState { get; set; } = null;

        public event StartedEventHandler Started;

        public event DoorEventHandler DoorOpened;
        public event DoorEventHandler DoorClosed;

        public event BeaconPassedEventHandler BeaconPassed;
    }
}
