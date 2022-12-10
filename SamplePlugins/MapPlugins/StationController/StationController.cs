﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.Extensions.ContextMenuHacker;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Samples.MapPlugins.StationController
{
    [PluginType(PluginType.MapPlugin)]
    public class StationController : AssemblyPluginBase, IDisposable
    {
        private readonly ControllerForm Form;
        private readonly ToolStripMenuItem MenuItem;

        public StationController(PluginBuilder services) : base(services)
        {
            InstanceStore.Initialize(Native, Extensions, BveHacker);

            IContextMenuHacker contextMenuHacker = Extensions.GetExtension<IContextMenuHacker>();
            MenuItem = contextMenuHacker.AddCheckableMenuItem("駅編集ウィンドウを表示", MenuItemCheckedChanged, ContextMenuItemType.Plugins);

            MenuItem.Checked = false;

            Form = new ControllerForm();
            Form.FormClosing += FormClosing;
            Form.WindowState = FormWindowState.Normal;

            MenuItem.Checked = true;
            BveHacker.MainFormSource.Focus();
        }

        public override void Dispose()
        {
            Form.FormClosing -= FormClosing;
            Form.Close();
            Form.Dispose();
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            return new MapPluginTickResult();
        }

        private void MenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (MenuItem.Checked)
            {
                Form.Show(BveHacker.MainFormSource);
            }
            else
            {
                Form.Hide();
            }
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            MenuItem.Checked = false;
        }
    }
}
