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
    /// すべてのブレーキ方式の基本クラスを表します。
    /// </summary>
    public class BrakeControllerBase : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeControllerBase>();

            MaximumPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(MaximumPressure));
            MaximumPressureSetMethod = members.GetSourcePropertySetterOf(nameof(MaximumPressure));

            PressureRatesGetMethod = members.GetSourcePropertyGetterOf(nameof(PressureRates));
            PressureRatesSetMethod = members.GetSourcePropertySetterOf(nameof(PressureRates));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakeControllerBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakeControllerBase(object src) : base(src)
        {
        }

        private static FastMethod MaximumPressureGetMethod;
        private static FastMethod MaximumPressureSetMethod;
        /// <summary>
        /// 常用最大ブレーキ時のブレーキシリンダー圧力指令 (空車時) [Pa] を取得・設定します。<br/>
        /// 電磁直通空気ブレーキの場合は直通管圧力を表します。
        /// </summary>
        public double MaximumPressure
        {
            get => MaximumPressureGetMethod.Invoke(Src, null);
            set => MaximumPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PressureRatesGetMethod;
        private static FastMethod PressureRatesSetMethod;
        /// <summary>
        /// MaximumPressure を 1 としたときの各ブレーキノッチの圧力指令を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 電気指令式ブレーキおよび電磁直通空気ブレーキの場合に限り認識されます。
        /// </remarks>
        public double[] PressureRates
        {
            get => PressureRatesGetMethod.Invoke(Src, null);
            set => PressureRatesSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
