﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Automatic9045.AtsEx.PluginHost;
using Automatic9045.AtsEx.PluginHost.ClassWrappers;
using Automatic9045.AtsEx.PluginHost.Extensions;

namespace Automatic9045.MapPlugins.StationController
{
    public partial class ControllerForm : Form
    {
        public ControllerForm()
        {
            InitializeComponent();

            if (InstanceStore.Instance.BveHacker.IsScenarioCreated)
            {
                ResetInput();
            }
            else
            {
                InstanceStore.Instance.BveHacker.ScenarioCreated += ScenarioCreated;
            }

            void ScenarioCreated(ScenarioCreatedEventArgs e)
            {
                InstanceStore.Instance.BveHacker.ScenarioCreated -= ScenarioCreated;
                ResetInput(e.Scenario);
            }
        }

        private void ResetInput(Scenario scenario = null)
        {
            scenario = scenario ?? InstanceStore.Instance.BveHacker.Scenario;

            StationList stations = scenario.Route.Stations;
            Station lastStation = stations.Count == 0 ? null : stations[stations.Count - 1] as Station;
            
            LocationValue.Text = (lastStation is null ? 0 : lastStation.Location + 500).ToString();
            int arrivalTime = lastStation is null ? 10 * 60 * 60 * 1000 : lastStation.DefaultTimeMilliseconds + 2 * 60 * 1000;
            ArrivalTimeValue.Text = arrivalTime.ToTimeText();
            DepertureTimeValue.Text = (arrivalTime + 30 * 1000).ToTimeText();
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            StationList stations = InstanceStore.Instance.BveHacker.Scenario.Route.Stations;

            try
            {
                Station newStation = new Station(NameValue.Text)
                {
                    Location = int.Parse(LocationValue.Text),
                    DefaultTimeMilliseconds = ArrivalTimeValue.Text.ToTimeMilliseconds(),
                    ArrivalTimeMilliseconds = ArrivalTimeValue.Text.ToTimeMilliseconds(),
                    DepertureTimeMilliseconds = Pass.Checked ? int.MaxValue : DepertureTimeValue.Text.ToTimeMilliseconds(),
                    Pass = Pass.Checked,
                    IsTerminal = IsTerminal.Checked,
                };
                stations.Add(newStation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生したため駅を追加できませんでした。\n\n詳細：\n\n" + ex.ToString(), "駅追加エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetInput();
                return;
            }

            InstanceStore.Instance.BveHacker.UpdateDiagram();
            ResetInput();
        }

        private void RemoveButtonClicked(object sender, EventArgs e)
        {
            StationList stations = InstanceStore.Instance.BveHacker.Scenario.Route.Stations;
            if (stations.Count == 0) return;
            stations.RemoveAt(stations.Count - 1);

            if (stations.Count == 0)
            {
                RemoveButton.Enabled = false;
            }
            else
            {
                Station lastStation = stations.Last() as Station;
            }

            InstanceStore.Instance.BveHacker.UpdateDiagram();
        }
    }
}
