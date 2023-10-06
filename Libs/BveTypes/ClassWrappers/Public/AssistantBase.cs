﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mackoy.Bvets;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// すべての補助表示の基本クラスを表します。
    /// </summary>
    public class AssistantBase : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AssistantBase>();

            AssistantSettingsGetMethod = members.GetSourcePropertyGetterOf(nameof(AssistantSettings));

            BackgroundColorGetMethod = members.GetSourcePropertyGetterOf(nameof(BackgroundColor));
            BackgroundColorSetMethod = members.GetSourcePropertySetterOf(nameof(BackgroundColor));

            DisplayAreaGetMethod = members.GetSourcePropertyGetterOf(nameof(DisplayArea));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AssistantBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AssistantBase(object src) : base(src)
        {
        }

        private static FastMethod AssistantSettingsGetMethod;
        /// <summary>
        /// 補助表示の設定を取得します。
        /// </summary>
        public AssistantSettings AssistantSettings => AssistantSettingsGetMethod.Invoke(Src, null);

        private static FastMethod BackgroundColorGetMethod;
        private static FastMethod BackgroundColorSetMethod;
        /// <summary>
        /// 補助表示の背景色を取得・設定します。
        /// </summary>
        public Color BackgroundColor
        {
            get => BackgroundColorGetMethod.Invoke(Src, null);
            set => BackgroundColorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DisplayAreaGetMethod;
        /// <summary>
        /// 表示する位置とサイズを表す <see cref="Rectangle"/> を取得します。
        /// </summary>
        public Rectangle DisplayArea => DisplayAreaGetMethod.Invoke(Src, null);

        private static FastMethod DrawMethod;
        /// <summary>
        /// 補助表示を描画します。
        /// </summary>
        public void Draw()
        {
            DrawMethod.Invoke(Src, null);
        }
    }
}
