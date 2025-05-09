﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 補助表示のセットを表します。
    /// </summary>
    public class AssistantSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AssistantSet>();

            ItemsGetMethod = members.GetSourcePropertyGetterOf(nameof(Items));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            OnDeviceLostMethod = members.GetSourceMethodOf(nameof(OnDeviceLost));
            OnDeviceResetMethod = members.GetSourceMethodOf(nameof(OnDeviceReset));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AssistantSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AssistantSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="AssistantText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static AssistantSet FromSource(object src) => src is null ? null : new AssistantSet(src);

        private static FastMethod ItemsGetMethod;
        /// <summary>
        /// 補助表示の一覧を取得します。
        /// </summary>
        public WrappedList<AssistantBase> Items => WrappedList<AssistantBase>.FromSource(ItemsGetMethod.Invoke(Src, null) as IList);

        private static FastMethod DrawMethod;
        /// <summary>
        /// 全ての補助表示を描画します。
        /// </summary>
        public void Draw() => DrawMethod.Invoke(Src, null);

        private static FastMethod OnDeviceLostMethod;
        /// <inheritdoc/>
        public void OnDeviceLost() => OnDeviceLostMethod.Invoke(Src, null);

        private static FastMethod OnDeviceResetMethod;
        /// <inheritdoc/>
        public void OnDeviceReset() => OnDeviceResetMethod.Invoke(Src, null);
    }
}
