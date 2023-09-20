﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 設置距離程 25m おきにストラクチャーをグルーピングしたブロックを表します。
    /// </summary>
    public class StructureBlock : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StructureBlock>();

            Constructor = members.GetSourceConstructor();

            DirectionGetMethod = members.GetSourcePropertyGetterOf(nameof(Direction));
            DirectionSetMethod = members.GetSourcePropertySetterOf(nameof(Direction));

            ModelsGetMethod = members.GetSourcePropertyGetterOf(nameof(Models));

            WorldMatrixGetMethod = members.GetSourcePropertyGetterOf(nameof(WorldMatrix));
            WorldMatrixSetMethod = members.GetSourcePropertySetterOf(nameof(WorldMatrix));

            MatrixFromLastBlockGetMethod = members.GetSourcePropertyGetterOf(nameof(MatrixFromLastBlock));
            MatrixFromLastBlockSetMethod = members.GetSourcePropertySetterOf(nameof(MatrixFromLastBlock));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StructureBlock"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StructureBlock(object src) : base(src)
        {
        }

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="StructureBlock"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public StructureBlock() : base(Constructor.Invoke(null))
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StructureBlock"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static StructureBlock FromSource(object src) => src is null ? null : new StructureBlock(src);

        private static FastMethod DirectionGetMethod;
        private static FastMethod DirectionSetMethod;
        /// <summary>
        /// このストラクチャーブロックの原点において、自軌道が向いている方角 [rad] を取得・設定します。
        /// </summary>
        public double Direction
        {
            get => DirectionGetMethod.Invoke(Src, null);
            set => DirectionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ModelsGetMethod;
        /// <summary>
        /// このストラクチャーブロックに配置されている 3D モデルの一覧を取得・設定します。
        /// </summary>
        public WrappedList<LocatableModel> Models
        {
            get
            {
                IList listSrc = ModelsGetMethod.Invoke(Src, null);
                return WrappedList<LocatableModel>.FromSource(listSrc);
            }
        }

        private static FastMethod WorldMatrixGetMethod;
        private static FastMethod WorldMatrixSetMethod;
        /// <summary>
        /// このストラクチャーブロックを配置するためのワールド変換行列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// プロパティの値は、このストラクチャーブロックが描画範囲内にあるときに限って更新されます。
        /// </remarks>
        public Matrix WorldMatrix
        {
            get => WorldMatrixGetMethod.Invoke(Src, null);
            set => WorldMatrixSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MatrixFromLastBlockGetMethod;
        private static FastMethod MatrixFromLastBlockSetMethod;
        /// <summary>
        /// 1 つ前のストラクチャーブロックから見たときの、このストラクチャーブロックの相対位置を表すワールド変換行列を取得・設定します。<br/>
        /// 1 つ前のストラクチャーブロック、このストラクチャーブロックの <see cref="WorldMatrix"/> をそれぞれ M1、M2 とすると、プロパティの値は M2 = M1 * ΔM を満たす ΔM に当たります。
        /// </summary>
        public Matrix MatrixFromLastBlock
        {
            get => MatrixFromLastBlockGetMethod.Invoke(Src, null);
            set => MatrixFromLastBlockSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DrawMethod;
        /// <summary>
        /// 描画範囲内のストラクチャーを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="additionalWorldMatrix">ワールド変換行列の後に追加で掛ける行列。</param>
        /// <param name="isZDisabled">Z バッファを無効にして描画するかどうか。</param>
        public void Draw(Direct3DProvider direct3DProvider, Matrix additionalWorldMatrix, bool isZDisabled)
            => DrawMethod.Invoke(Src, new object[] { direct3DProvider.Src, additionalWorldMatrix, isZDisabled });
    }
}
