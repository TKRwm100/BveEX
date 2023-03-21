﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.PluginHost.Plugins;

using VehiclePlugin = AtsEx.Samples.VehiclePlugins.CooperatingWithMapPlugin.PluginMain;

namespace AtsEx.Samples.MapPlugins.CooperatingWithVehiclePlugin
{
    [PluginType(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Plugins.AllPluginsLoaded += OnAllPluginsLoaded;
        }

        private void OnAllPluginsLoaded(object sender, EventArgs e)
        {
            VehiclePlugin vehiclePlugin = Plugins[PluginType.VehiclePlugin]["TestPlugin"] as VehiclePlugin;
            MessageBox.Show($"車両プラグインから値を取得しました: {vehiclePlugin.SharedValue}", "AtsEX マッププラグイン：車両プラグイン連携サンプル");
        }

        public override void Dispose()
        {
            Plugins.AllPluginsLoaded -= OnAllPluginsLoaded;
        }

        public override TickResult Tick(TimeSpan elapsed) => new MapPluginTickResult();
    }
}
