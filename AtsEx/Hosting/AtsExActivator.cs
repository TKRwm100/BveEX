﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automatic9045.AtsEx.Hosting
{
    internal class AtsExActivator
    {
        public Process TargetProcess { get; }
        public AppDomain TargetAppDomain { get; }
        public Assembly TargetAssembly { get; }

        private readonly Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

        public AtsExActivator()
        {
            TargetProcess = Process.GetCurrentProcess();
            TargetAppDomain = AppDomain.CurrentDomain;
            TargetAssembly = Assembly.GetEntryAssembly();

            CheckAssembly();
        }

        public async Task CheckUpdatesAsync()
        {
            try
            {
                AtsExRepositoryHost repositoryHost = new AtsExRepositoryHost();

                Version latestVersion = await repositoryHost.GetLatestVersionAsync().ConfigureAwait(false);
                Version currentVersion = ExecutingAssembly.GetName().Version;

                if (currentVersion < latestVersion)
                {
                    DialogResult confirm = MessageBox.Show($"新しいバージョンの AtsEX がリリースされています。" +
                        $"\n\n現在のバージョン：{currentVersion}\n最新のバージョン：{latestVersion}\n\n" +
                        $"リリースページを開きますか？", "AtsEX", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        Process.Start("https://github.com/automatic9045/AtsEX/releases");
                    }
                }
            }
            catch
            {
            }
        }

        private void CheckAssembly()
        {
            if (TargetAssembly is null)
            {
                ShowErrorDialog("BVE 本体が読み込めないフォーマットです。");
            }
            else if (!TargetAssembly.GetTypes().Any(t => t.Namespace == "Mackoy.Bvets"))
            {
                ShowErrorDialog("BVE 本体と異なるプロセスで実行することはできません。", "https://automatic9045.github.io/contents/bve/AtsEX/faq/#diff-process");
            }
        }

        private void ShowErrorDialog(string message, string faqUrl = null)
        {
            if (faqUrl is null)
            {
                MessageBox.Show(message, $"エラー - AtsEX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show($"{message}\n\nこのエラーに関する情報を表示しますか？\n（ブラウザで Web サイトが開きます）", $"エラー - AtsEX", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start(faqUrl);
                }
            }

            throw new NotSupportedException(message);
        }
    }
}
