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
    /// 定速制御を行うためのリレーを表します。
    /// </summary>
    public class ConstantSpeedRelay : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ConstantSpeedRelay>();

            NeutralControlModeGetMethod = members.GetSourcePropertyGetterOf(nameof(NeutralControlMode));
            NeutralControlModeSetMethod = members.GetSourcePropertySetterOf(nameof(NeutralControlMode));

            PowerControlModeGetMethod = members.GetSourcePropertyGetterOf(nameof(PowerControlMode));
            PowerControlModeSetMethod = members.GetSourcePropertySetterOf(nameof(PowerControlMode));

            BrakeControlModeGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeControlMode));
            BrakeControlModeSetMethod = members.GetSourcePropertySetterOf(nameof(BrakeControlMode));

            MinSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(MinSpeed));
            MinSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(MinSpeed));

            InputField = members.GetSourceFieldOf(nameof(Input));
            OutputField = members.GetSourceFieldOf(nameof(Output));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ConstantSpeedRelay"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ConstantSpeedRelay(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ConstantSpeedRelay"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ConstantSpeedRelay FromSource(object src) => src is null ? null : new ConstantSpeedRelay(src);

        private static FastMethod NeutralControlModeGetMethod;
        private static FastMethod NeutralControlModeSetMethod;
        /// <summary>
        /// 惰行中に定速スイッチが扱われた場合の制御種別を取得・設定します。
        /// </summary>
        public int NeutralControlMode
        {
            get => (int)NeutralControlModeGetMethod.Invoke(Src, null);
            set => NeutralControlModeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PowerControlModeGetMethod;
        private static FastMethod PowerControlModeSetMethod;
        /// <summary>
        /// 力行中に定速スイッチが扱われた場合の制御種別を取得・設定します。
        /// </summary>
        public int PowerControlMode
        {
            get => (int)PowerControlModeGetMethod.Invoke(Src, null);
            set => PowerControlModeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BrakeControlModeGetMethod;
        private static FastMethod BrakeControlModeSetMethod;
        /// <summary>
        /// ブレーキ中に定速スイッチが扱われた場合の制御種別を取得・設定します。
        /// </summary>
        public int BrakeControlMode
        {
            get => (int)BrakeControlModeGetMethod.Invoke(Src, null);
            set => BrakeControlModeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MinSpeedGetMethod;
        private static FastMethod MinSpeedSetMethod;
        /// <summary>
        /// 定速制御が作動する最低速度 [m/s] を取得・設定します。
        /// </summary>
        public double MinSpeed
        {
            get => (double)MinSpeedGetMethod.Invoke(Src, null);
            set => MinSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField InputField;
        /// <summary>
        /// ハンドル入力を取得・設定します。
        /// </summary>
        public HandleSet Input
        {
            get => HandleSet.FromSource(InputField.GetValue(Src));
            set => InputField.SetValue(Src, value?.Src);
        }

        private static FastField OutputField;
        /// <summary>
        /// ハンドル出力を取得・設定します。
        /// </summary>
        public HandleSet Output
        {
            get => HandleSet.FromSource(OutputField.GetValue(Src));
            set => OutputField.SetValue(Src, value?.Src);
        }

        private static FastMethod InitializeMethod;
        /// <summary>
        /// 初期化します。
        /// </summary>
        public void Initialize() => InitializeMethod.Invoke(Src, null);
    }
}
