﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 補助表示を描画するための機能を提供します。
    /// </summary>
    public class AssistantDrawer : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AssistantDrawer>();

            ItemsGetMethod = members.GetSourcePropertyGetterOf(nameof(Items));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AssistantDrawer"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AssistantDrawer(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="AssistantText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static AssistantDrawer FromSource(object src) => src is null ? null : new AssistantDrawer(src);

        private static FastMethod ItemsGetMethod;
        /// <summary>
        /// 補助表示の一覧を取得します。
        /// </summary>
        public WrappedList<AssistantBase> Items => WrappedList<AssistantBase>.FromSource(ItemsGetMethod.Invoke(Src, null));
    }
}
