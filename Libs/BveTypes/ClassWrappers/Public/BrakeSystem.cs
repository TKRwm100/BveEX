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
    /// 自列車のブレーキシステム全体を表します。
    /// </summary>
    public class BrakeSystem : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeSystem>();

            ElectroPneumaticBlendedBrakingControlGetMethod = members.GetSourcePropertyGetterOf(nameof(ElectroPneumaticBlendedBrakingControl));
            AirSupplementGetMethod = members.GetSourcePropertyGetterOf(nameof(AirSupplement));
            LockoutValveGetMethod = members.GetSourcePropertyGetterOf(nameof(LockoutValve));

            BrakeControllerGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeController));
            EcbGetMethod = members.GetSourcePropertyGetterOf(nameof(Ecb));
            SmeeGetMethod = members.GetSourcePropertyGetterOf(nameof(Smee));
            ClGetMethod = members.GetSourcePropertyGetterOf(nameof(Cl));

            MotorCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCarBc));

            TrailerCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCarBc));

            FirstCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCarBc));
            FirstCarBcSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCarBc));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakeSystem"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakeSystem(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BrakeSystem"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BrakeSystem FromSource(object src) => src is null ? null : new BrakeSystem(src);

        private static FastMethod ElectroPneumaticBlendedBrakingControlGetMethod;
        /// <summary>
        /// 自列車が使用する電空協調制御を取得します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="AirSupplement"/> プロパティ、<see cref="LockoutValve"/> プロパティのどちらかとなります。
        /// </remarks>
        public ElectroPneumaticBlendedBrakingControlBase ElectroPneumaticBlendedBrakingControl => CreateFromSource(ElectroPneumaticBlendedBrakingControlGetMethod.Invoke(Src, null));

        private static FastMethod AirSupplementGetMethod;
        /// <summary>
        /// 遅れ込め制御式電空協調制御を取得します。
        /// </summary>
        public AirSupplement AirSupplement => ClassWrappers.AirSupplement.FromSource(AirSupplementGetMethod.Invoke(Src, null));

        private static FastMethod LockoutValveGetMethod;
        /// <summary>
        /// 締切電磁弁式電空協調制御を取得します。
        /// </summary>
        public AirSupplement LockoutValve => ClassWrappers.AirSupplement.FromSource(LockoutValveGetMethod.Invoke(Src, null));

        private static FastMethod BrakeControllerGetMethod;
        /// <summary>
        /// 自列車が使用するブレーキ方式を取得します。
        /// </summary>
        /// <remarks>
        /// 取得される値は、パラメーターファイルでの設定に合わせて <see cref="Ecb"/> プロパティ、<see cref="Smee"/> プロパティ、<see cref="Cl"/> プロパティのいずれかとなります。
        /// </remarks>
        public BrakeControllerBase BrakeController => CreateFromSource(BrakeControllerGetMethod.Invoke(Src, null));

        private static FastMethod EcbGetMethod;
        /// <summary>
        /// 電気指令式ブレーキを取得します。
        /// </summary>
        public Ecb Ecb => ClassWrappers.Ecb.FromSource(EcbGetMethod.Invoke(Src, null));

        private static FastMethod SmeeGetMethod;
        /// <summary>
        /// 電磁直通空気ブレーキを取得します。
        /// </summary>
        public Smee Smee => ClassWrappers.Smee.FromSource(SmeeGetMethod.Invoke(Src, null));

        private static FastMethod ClGetMethod;
        /// <summary>
        /// 自動空気ブレーキを取得します。
        /// </summary>
        public Cl Cl => ClassWrappers.Cl.FromSource(ClGetMethod.Invoke(Src, null));

        private static FastMethod MotorCarBcGetMethod;
        /// <summary>
        /// 動力車のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得します。
        /// </summary>
        public CarBc MotorCarBc => CarBc.FromSource(MotorCarBcGetMethod.Invoke(Src, null));

        private static FastMethod TrailerCarBcGetMethod;
        /// <summary>
        /// 付随車のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得します。
        /// </summary>
        public CarBc TrailerCarBc => CarBc.FromSource(TrailerCarBcGetMethod.Invoke(Src, null));

        private static FastMethod FirstCarBcGetMethod;
        private static FastMethod FirstCarBcSetMethod;
        /// <summary>
        /// 先頭車両のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得・設定します。
        /// </summary>
        public CarBc FirstCarBc
        {
            get => CarBc.FromSource(FirstCarBcGetMethod.Invoke(Src, null));
            set => FirstCarBcSetMethod.Invoke(Src, value.Src);
        }
    }
}
