﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using BveTypes.ClassWrappers;
using ObjectiveHarmonyPatch;

using AtsEx.Native;

namespace AtsEx
{
    internal abstract partial class AtsEx
    {
        internal partial class AsInputDevice
        {
            private class PatchEventInitializer
            {
                private readonly AsInputDevice Target;

                public PatchEventInitializer(AsInputDevice target)
                {
                    Target = target;
                }

                public void InitializeEvents()
                {
                    Target.Patches.LoadScenarioPatch.Invoked += (sender, e) =>
                    {
                        ScenarioInfo scenarioInfo = ScenarioInfo.FromSource(e.Args[0]);

                        Target.ScenarioOpened?.Invoke(this, new ValueEventArgs<ScenarioInfo>(scenarioInfo));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.DisposeScenarioPatch.Invoked += (sender, e) =>
                    {
                        Target.ScenarioClosed?.Invoke(this, EventArgs.Empty);
                        return new PatchInvokationResult(SkipModes.Continue);
                    };


                    Target.Patches.OnSetBeaconDataPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);
                        ObjectPassedEventArgs args = ObjectPassedEventArgs.FromSource(e.Args[1]);

                        Beacon beacon = (Beacon)args.MapObject;
                        Section section = beacon.TargetSectionIndex == int.MaxValue
                            ? pluginLoader.SectionManager.LastSection
                            : beacon.TargetSectionIndex >= 0 && pluginLoader.SectionManager.Sections.Count > beacon.TargetSectionIndex ? (Section)pluginLoader.SectionManager.Sections[beacon.TargetSectionIndex] : null;

                        BeaconData beaconData = new BeaconData()
                        {
                            Data = beacon.SendData,
                            Z = section is null ? 0f : (float)(section.Location - pluginLoader.LocationManager.Location),
                            Sig = section is null ? 0 : section.CurrentSignalIndex,
                            Num = beacon.Type,
                        };

                        Target.OnSetBeaconData?.Invoke(this, new ValueEventArgs<BeaconData>(beaconData));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnKeyDownPatch.Invoked += (sender, e) =>
                    {
                        InputEventArgs args = (InputEventArgs)e.Args[1];

                        int axis = args.Axis;
                        int value = args.Value & 65535;

                        switch (axis)
                        {
                            case -1:
                                switch (value)
                                {
                                    case 0:
                                        Target.OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Primary));
                                        break;

                                    case 1:
                                        Target.OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Secondary));
                                        break;

                                    case 3:
                                        Target.OnHornBlow?.Invoke(this, new ValueEventArgs<HornType>(HornType.Music));
                                        break;
                                }
                                break;

                            case -2:
                                Target.OnKeyDown?.Invoke(this, new ValueEventArgs<ATSKeys>((ATSKeys)value));
                                break;
                        }

                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnKeyUpPatch.Invoked += (sender, e) =>
                    {
                        InputEventArgs args = (InputEventArgs)e.Args[1];

                        int axis = args.Axis;
                        int value = args.Value & 65535;

                        if (axis == -2)
                        {
                            Target.OnKeyUp?.Invoke(this, new ValueEventArgs<ATSKeys>((ATSKeys)value));
                        }
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnDoorStateChangedPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);

                        if (pluginLoader.Doors.AreAllClosingOrClosed)
                        {
                            Target.OnDoorClose?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            Target.OnDoorOpen?.Invoke(this, EventArgs.Empty);
                        }
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnSetSignalPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);
                        MapFunctionList sections = pluginLoader.SectionManager.Sections;

                        int currentSectionSignalIndex = sections.CurrentIndex < 0 ? int.MaxValue : ((Section)sections[sections.CurrentIndex]).CurrentSignalIndex;

                        Target.OnSetSignal?.Invoke(this, new ValueEventArgs<int>(currentSectionSignalIndex));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnSetReverserPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);

                        Target.OnSetReverser?.Invoke(this, new ValueEventArgs<int>((int)pluginLoader.Handles.ReverserPosition));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnSetBrakePatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);

                        Target.OnSetBrake?.Invoke(this, new ValueEventArgs<int>(pluginLoader.Handles.BrakeNotch));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnSetPowerPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);

                        Target.OnSetPower?.Invoke(this, new ValueEventArgs<int>(pluginLoader.Handles.PowerNotch));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnSetVehicleSpecPatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);
                        if (!pluginLoader.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                        NotchInfo notchInfo = NotchInfo.FromSource(e.Args[0]);
                        int carCount = (int)e.Args[1];

                        VehicleSpec vehicleSpec = new VehicleSpec()
                        {
                            PowerNotches = notchInfo.PowerNotchCount,
                            BrakeNotches = notchInfo.BrakeNotchCount,
                            B67Notch = notchInfo.B67Notch,
                            AtsNotch = notchInfo.AtsCancelNotch,
                            Cars = carCount,
                        };

                        Target.OnSetVehicleSpec?.Invoke(this, new ValueEventArgs<VehicleSpec>(vehicleSpec));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnInitializePatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);
                        if (!pluginLoader.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                        BrakePosition brakePosition = (BrakePosition)e.Args[0];

                        Target.OnInitialize?.Invoke(this, new ValueEventArgs<DefaultBrakePosition>((DefaultBrakePosition)brakePosition));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };

                    Target.Patches.OnElapsePatch.Invoked += (sender, e) =>
                    {
                        PluginLoader pluginLoader = PluginLoader.FromSource(e.Instance);
                        if (!pluginLoader.IsPluginLoaded) return new PatchInvokationResult(SkipModes.Continue);

                        VehicleState vehicleState = new VehicleState()
                        {
                            Location = pluginLoader.LocationManager.Location,
                            Speed = (float)pluginLoader.StateStore.Speed[0],
                            Time = (int)e.Args[0],
                            BcPressure = (float)pluginLoader.StateStore.BcPressure[0],
                            MrPressure = (float)pluginLoader.StateStore.MrPressure[0],
                            ErPressure = (float)pluginLoader.StateStore.ErPressure[0],
                            BpPressure = (float)pluginLoader.StateStore.BpPressure[0],
                            SapPressure = (float)pluginLoader.StateStore.SapPressure[0],
                            Current = (float)pluginLoader.StateStore.Current[0],
                        };

                        Target.OnElapse?.Invoke(this, new OnElapseEventArgs(vehicleState, pluginLoader.PanelArray, pluginLoader.SoundArray));
                        return new PatchInvokationResult(SkipModes.Continue);
                    };
                }
            }
        }
    }
}
