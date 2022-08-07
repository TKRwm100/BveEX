﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Automatic9045.AtsEx.PluginHost.BveTypes;

namespace Automatic9045.AtsEx.PluginHost.ClassWrappers
{
    /// <summary>
    /// BVE 標準機能の時刻表を表します。
    /// </summary>
    /// <seealso cref="DiagramUpdater"/>
    public class TimeTable : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TimeTable>();

            ModelField = members.GetSourceFieldOf(nameof(Model));
            NameTextsField = members.GetSourceFieldOf(nameof(NameTexts));
            ArrivalTimeTextsField = members.GetSourceFieldOf(nameof(ArrivalTimeTexts));
            DepertureTimeTextsField = members.GetSourceFieldOf(nameof(DepertureTimeTexts));
            NameTextWidthsField = members.GetSourceFieldOf(nameof(NameTextWidths));
            ArrivalTimeTextWidthsField = members.GetSourceFieldOf(nameof(ArrivalTimeTextWidths));
            DepertureTimeTextWidthsField = members.GetSourceFieldOf(nameof(DepertureTimeTextWidths));

            UpdateMethod = members.GetSourceMethodOf(nameof(Update));
        }

        protected TimeTable(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimeTable"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TimeTable FromSource(object src)
        {
            if (src is null) return null;
            return new TimeTable(src);
        }

        protected static FieldInfo ModelField;
        /// <summary>
        /// 時刻表を表示するための 2D モデルを表す <see cref="ClassWrappers.Model"/> を取得・設定します。
        /// </summary>
        public Model Model
        {
            get => ModelField.GetValue(Src);
            set => ModelField.SetValue(Src, value);
        }

        protected static FieldInfo NameTextsField;
        /// <summary>
        /// 表示する停車場名の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("停車場名") も含まれます。
        /// </remarks>
        /// <seealso cref="NameTextWidths"/>
        public string[] NameTexts
        {
            get => NameTextsField.GetValue(Src);
            set => NameTextsField.SetValue(Src, value);
        }

        protected static FieldInfo ArrivalTimeTextsField;
        /// <summary>
        /// 表示する到着時刻の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("着") も含まれます。
        /// </remarks>
        /// <seealso cref="ArrivalTimeTextWidths"/>
        public string[] ArrivalTimeTexts
        {
            get => ArrivalTimeTextsField.GetValue(Src);
            set => ArrivalTimeTextsField.SetValue(Src, value);
        }

        protected static FieldInfo DepertureTimeTextsField;
        /// <summary>
        /// 表示する発車時刻または通過時刻の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("発(通)") も含まれます。
        /// </remarks>
        /// <seealso cref="DepertureTimeTextWidths"/>
        public string[] DepertureTimeTexts
        {
            get => DepertureTimeTextsField.GetValue(Src);
            set => DepertureTimeTextsField.SetValue(Src, value);
        }

        protected static FieldInfo NameTextWidthsField;
        /// <summary>
        /// 停車場名の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("停車場名") も含まれます。
        /// </remarks>
        /// <seealso cref="NameTexts"/>
        public int[] NameTextWidths
        {
            get => NameTextWidthsField.GetValue(Src);
            set => NameTextWidthsField.SetValue(Src, value);
        }

        protected static FieldInfo ArrivalTimeTextWidthsField;
        /// <summary>
        /// 到着時刻の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("着") も含まれます。
        /// </remarks>
        /// <seealso cref="ArrivalTimeTexts"/>
        public int[] ArrivalTimeTextWidths
        {
            get => ArrivalTimeTextWidthsField.GetValue(Src);
            set => ArrivalTimeTextWidthsField.SetValue(Src, value);
        }

        protected static FieldInfo DepertureTimeTextWidthsField;
        /// <summary>
        /// 発車時刻または通過時刻の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("発(通)") も含まれます。
        /// </remarks>
        /// <seealso cref="DepertureTimeTexts"/>
        public int[] DepertureTimeTextWidths
        {
            get => DepertureTimeTextWidthsField.GetValue(Src);
            set => DepertureTimeTextWidthsField.SetValue(Src, value);
        }

        protected static MethodInfo UpdateMethod;
        /// <summary>
        /// 時刻表の表示を最新の状態に更新します。
        /// </summary>
        /// <remarks>
        /// このメソッドは独自の更新処理を実装するために公開されているものです。通常は <see cref="DiagramUpdater"/> を使用してください。
        /// </remarks>
        /// <seealso cref="DiagramUpdater"/>
        /// <seealso cref="DiagramUpdater.UpdateDiagram"/>
        public void Update()
        {
            UpdateMethod.Invoke(Src, null);
        }
    }
}
