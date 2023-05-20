﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using AtsEx.Extensions.ContextMenuHacker;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;

namespace AtsEx
{
    internal sealed class VersionFormProvider : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<VersionFormProvider>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> VersionInfoMenuItem { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static VersionFormProvider()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly Form MainFormSource;

        private readonly IEnumerable<PluginBase> Extensions;

        private readonly VersionForm Form;
        private readonly ToolStripMenuItem MenuItem;


        public VersionFormProvider(Form mainFormSource, IEnumerable<PluginBase> extensions, IContextMenuHacker contextMenuHacker)
        {
            MainFormSource = mainFormSource;

            Extensions = extensions;

            string versionInfoMenuItemText = string.Format(Resources.Value.VersionInfoMenuItem.Value, App.Instance.ProductShortName);
            MenuItem = contextMenuHacker.AddClickableMenuItem(versionInfoMenuItemText, MenuItemClick, ContextMenuItemType.CoreAndExtensions);

            Form = new VersionForm();
            Form.SetPluginDetails(Extensions);
            Form.FormClosing += FormClosing;
        }

        private void MenuItemClick(object sender, EventArgs e)
        {
            if (!Form.Visible)
            {
                Form.Show(MainFormSource);
            }

            Form.Focus();
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form.Hide();
        }

        public void ShowForm() => Form.Show(MainFormSource);

        public void SetScenario(IEnumerable<PluginBase> plugins) => Form.SetPluginDetails(Extensions.Concat(plugins));

        public void UnsetScenario() => Form.SetPluginDetails(Extensions);

        public void Dispose()
        {
            Form.FormClosing -= FormClosing;
            Form.Close();
            Form.Dispose();
        }
    }
}
