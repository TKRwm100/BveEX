﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mackoy.Bvets;

using BveTypes;
using BveTypes.ClassWrappers;

using AtsEx.PluginHost.LoadErrorManager;
using AtsEx.PluginHost.MapStatements;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// <see cref="IBveHacker.ScenarioCreated"/> または <see cref="IBveHacker.PreviewScenarioCreated"/> イベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="e">イベント データを格納している <see cref="ScenarioCreatedEventArgs"/>。</param>
    public delegate void ScenarioCreatedEventHandler(ScenarioCreatedEventArgs e);

    public interface IBveHacker : IDisposable
    {
        /// <summary>
        /// クラスラッパーに対応する BVE の型とメンバーの定義を表す <see cref="BveTypeSet"/> を取得します。
        /// </summary>
        BveTypeSet BveTypes { get; }


        /// <summary>
        /// BVE のメインフォームのハンドルを取得します。
        /// </summary>
        IntPtr MainFormHandle { get; }

        /// <summary>
        /// BVE のメインフォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form MainFormSource { get; }

        /// <summary>
        /// BVE のメインフォームを取得します。
        /// </summary>
        MainForm MainForm { get; }


        /// <summary>
        /// BVE の「シナリオの選択」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form ScenarioSelectionFormSource { get; }

        /// <summary>
        /// BVE の「シナリオの選択」フォームを取得します。
        /// </summary>
        ScenarioSelectionForm ScenarioSelectionForm { get; }

        /// <summary>
        /// BVE の「シナリオを読み込んでいます...」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form LoadingProgressFormSource { get; }

        /// <summary>
        /// BVE の「シナリオを読み込んでいます...」フォームを取得します。
        /// </summary>
        LoadingProgressForm LoadingProgressForm { get; }

        /// <summary>
        /// BVE の「時刻と位置」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form TimePosFormSource { get; }

        /// <summary>
        /// BVE の「時刻と位置」フォームを取得します。
        /// </summary>
        TimePosForm TimePosForm { get; }

        /// <summary>
        /// BVE の「車両物理量」フォームの <see cref="Form"/> インスタンスを取得します。
        /// </summary>
        Form ChartFormSource { get; }

        /// <summary>
        /// BVE の「車両物理量」フォームを取得します。
        /// </summary>
        ChartForm ChartForm { get; }


        /// <summary>
        /// BVE の設定が格納された <see cref="Mackoy.Bvets.Preferences"/> を取得します。
        /// </summary>
        Preferences Preferences { get; }

        /// <summary>
        /// キー入力を管理する <see cref="BveTypes.ClassWrappers.KeyProvider"/> を取得します。
        /// </summary>
        KeyProvider KeyProvider { get; }


        /// <summary>
        /// シナリオ読込時のエラーを編集するための機能を提供する <see cref="PluginHost.LoadErrorManager"/> を取得します。
        /// </summary>
        ILoadErrorManager LoadErrorManager { get; }


        /// <summary>
        /// 全てのハンドルのセットを取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="IBveHacker"/> が利用できない場合は <see cref="INative.Handles"/> プロパティを使用してください。
        /// ただし、<see cref="INative.Handles"/> プロパティに設定されている値は力行ハンドルの抑速ノッチ、ブレーキハンドルの抑速ブレーキノッチを無視したものになります。
        /// </remarks>
        /// <seealso cref="INative.Handles"/>
        Handles.HandleSet Handles { get; }


        /// <summary>
        /// マップに定義されているヘッダーの一覧を取得します。
        /// </summary>
        IHeaderSet MapHeaders { get; }

        /// <summary>
        /// マップに定義されているユーザー独自ステートメントの一覧を取得します。
        /// </summary>
        IStatementSet MapStatements { get; }


        /// <summary>
        /// <see cref="ScenarioCreated"/> が発生する直前に通知します。特に理由がなければ <see cref="ScenarioCreated"/> を使用してください。
        /// </summary>
        event ScenarioCreatedEventHandler PreviewScenarioCreated;

        /// <summary>
        /// <see cref="BveTypes.ClassWrappers.Scenario"/> のインスタンスが生成されたときに通知します。
        /// </summary>
        event ScenarioCreatedEventHandler ScenarioCreated;

        /// <summary>
        /// 現在読込中または実行中のシナリオの情報を取得・設定します。
        /// </summary>
        ScenarioInfo ScenarioInfo { get; }

        /// <summary>
        /// 現在実行中のシナリオを取得します。シナリオの読込中は <see cref="InvalidOperationException"/> をスローします。
        /// シナリオの読込中に <see cref="BveTypes.ClassWrappers.Scenario"/> を取得するには <see cref="ScenarioCreated"/> イベントを購読してください。
        /// </summary>
        Scenario Scenario { get; }

        /// <summary>
        /// <see cref="Scenario"/> が取得可能かどうかを取得します。
        /// </summary>
        bool IsScenarioCreated { get; }
    }
}
