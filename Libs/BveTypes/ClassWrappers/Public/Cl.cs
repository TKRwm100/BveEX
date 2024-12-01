﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自動空気ブレーキを表します。
    /// </summary>
    public class Cl : BrakeControllerBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Cl>();

            BpInitialPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(BpInitialPressure));
            BpInitialPressureSetMethod = members.GetSourcePropertySetterOf(nameof(BpInitialPressure));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Cl"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Cl(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Cl"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Cl FromSource(object src) => src is null ? null : new Cl(src);

        private static FastMethod BpInitialPressureGetMethod;
        private static FastMethod BpInitialPressureSetMethod;
        /// <summary>
        /// ブレーキ緩解時のブレーキ管圧力 [Pa] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 電磁直通空気ブレーキおよび自動空気ブレーキの場合に限り認識されます。
        /// </remarks>
        public double BpInitialPressure
        {
            get => (double)BpInitialPressureGetMethod.Invoke(Src, null);
            set => BpInitialPressureSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
