﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// 全ての AtsEX プラグインの基本クラスを表します。
    /// </summary>
    /// <remarks>
    /// このクラスを直接継承する必要があるのは、特殊な AtsEX プラグインの場合のみです。
    /// アセンブリ (.dll) 形式の通常の AtsEX プラグインでは <see cref="AssemblyPluginBase"/> を継承してください。
    /// </remarks>
    public abstract class PluginBase : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginBase>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotUseAtsExExtensions { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginTypeNotSpecified { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PluginBase()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        /// <summary>
        /// この AtsEX プラグインの種類を取得します。
        /// </summary>
        public PluginType PluginType { get; }

        /// <summary>
        /// この AtsEX プラグインが AtsEX 独自の特殊機能拡張 (<see cref="IBveHacker"/>、マッププラグインなど) を利用するかどうかを取得します。
        /// </summary>
        /// <remarks>
        /// <see langword="false"/> に設定されている場合、<see cref="BveHacker"/> プロパティは取得できません。
        /// </remarks>
        public bool UseBveHacker { get; }

        /// <summary>
        /// BVE が標準で提供する ATS プラグイン向けの機能のラッパーを取得します。
        /// </summary>
        protected INative Native { get; }

        /// <summary>
        /// 読み込まれた AtsEX 拡張機能の一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>AtsEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合)、<see cref="IExtensionSet.AllExtensionsLoaded"/> イベントが発生するまでは項目を取得できません。</item>
        /// <item>AtsEX 拡張機能以外の AtsEX プラグインは <see cref="Plugins"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        ///////// <seealso cref="Plugins"/>
        protected IExtensionSet Extensions { get; }

        /// <summary>
        /// 読み込まれた AtsEX プラグインの一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>AtsEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合) は取得できません。</item>
        /// <item>AtsEX 拡張機能は <see cref="Extensions"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Extensions"/>
        protected IPluginSet Plugins { get; }

        private readonly IBveHacker _BveHacker = null;
        /// <summary>
        /// 本来 ATS プラグインからは利用できない BVE 本体の諸機能へアクセスするための <see cref="IBveHacker"/> を取得します。
        /// </summary>
        protected IBveHacker BveHacker
            => UseBveHacker ? _BveHacker : throw new InvalidOperationException(string.Format(Resources.Value.CannotUseAtsExExtensions.Value, nameof(UseBveHacker)));

        /// <summary>
        /// PluginUsing ファイルで指定したこの  AtsEX プラグインの識別子を取得します。このプロパティの値は全プラグインにおいて一意であることが保証されています。
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// この AtsEX プラグインのファイルの完全パスを取得します。
        /// </summary>
        public abstract string Location { get; }

        /// <summary>
        /// この AtsEX プラグインのファイル名を取得します。
        /// </summary>
        /// <remarks>
        /// 通常はプラグイン パッケージ ファイルの名前と拡張子 (例: MyPlugin.dll) を表しますが、<br/>
        /// スクリプト プラグインの場合など、ファイル名でプラグインを判別できない場合 (例: Package.xml) は代替の文字列を使用することもできます。
        /// </remarks>
        public abstract string Name { get; }

        /// <summary>
        /// この AtsEX プラグインのタイトルを取得します。
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// この AtsEX プラグインのバージョンを表す文字列を取得します。
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// この AtsEX プラグインの説明を取得します。
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// この AtsEX プラグインの著作権表示を取得します。
        /// </summary>
        public abstract string Copyright { get; }

        /// <summary>
        /// AtsEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="builder">AtsEX から渡される BVE、AtsEX の情報。</param>
        /// <param name="pluginType">AtsEX プラグインの種類。</param>
        /// <param name="useBveHacker">AtsEX 独自の特殊機能拡張 (<see cref="IBveHacker"/>、マッププラグインなど) を利用するか。<br/>
        /// <see langword="false"/> を指定すると、<see cref="BveHacker"/> が取得できなくなる代わりに、BVE のバージョンの問題で AtsEX 拡張機能の読込に失敗した場合でもシナリオを開始できるようになります。<br/>
        /// マッププラグインでは <see langword="false"/> を指定することはできません。</param>
        public PluginBase(PluginBuilder builder, PluginType pluginType, bool useBveHacker)
        {
            PluginType = pluginType;
            UseBveHacker = useBveHacker;

            Native = builder.Native;
            Extensions = builder.Extensions;
            Plugins = builder.Plugins;
            _BveHacker = builder.BveHacker;
            Identifier = builder.Identifier;
        }

        /// <summary>
        /// AtsEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <see cref="PluginTypeAttribute"/> を付加して、プラグインの種類を指定してください。<br/>
        /// AtsEX 拡張機能を利用しないプラグインであることを指定するには、<see cref="DoNotUseBveHackerAttribute"/> を付加してください。
        /// </remarks>
        /// <param name="builder">AtsEX から渡される BVE、AtsEX の情報。</param>
        public PluginBase(PluginBuilder builder) : this(builder, default, default)
        {
            Type type = GetType();
            PluginType? pluginType = null;
            bool useBveHacker = true;
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                switch (attribute)
                {
                    case PluginTypeAttribute pluginTypeAttribute:
                        pluginType = pluginTypeAttribute.PluginType;
                        break;

                    case DoNotUseBveHackerAttribute doNotUseBveHackerAttribute:
                        useBveHacker = false;
                        break;
                }
            }
            if (pluginType is null) throw new InvalidOperationException(string.Format(Resources.Value.PluginTypeNotSpecified.Value, typeof(PluginTypeAttribute).FullName));

            PluginType = (PluginType)pluginType;
            UseBveHacker = useBveHacker;
        }

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <summary>
        /// 毎フレーム呼び出されます。ネイティブ ATS プラグインの Elapse(ATS_VEHICLESTATE vehicleState, int[] panel, int[] sound) に当たります。
        /// </summary>
        /// <param name="elapsed">前フレームから経過した時間。</param>
        /// <returns>
        /// このメソッドの実行結果を表す <see cref="TickResult"/>。<br/>
        /// 拡張機能では <see cref="ExtensionTickResult"/> を、車両プラグインでは <see cref="VehiclePluginTickResult"/> を、マッププラグインでは <see cref="MapPluginTickResult"/> を返してください。
        /// </returns>
        public abstract TickResult Tick(TimeSpan elapsed);
    }
}
