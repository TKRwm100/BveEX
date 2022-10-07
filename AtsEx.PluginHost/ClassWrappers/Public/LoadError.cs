﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using Automatic9045.AtsEx.PluginHost.BveTypes;

namespace Automatic9045.AtsEx.PluginHost.ClassWrappers
{
    /// <summary>
    /// シナリオの読込中に発生したエラーを表します。
    /// </summary>
    public class LoadError : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<LoadError>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(string), typeof(string), typeof(int), typeof(int) });

            TextGetMethod = members.GetSourcePropertyGetterOf(nameof(Text));
            TextSetMethod = members.GetSourcePropertySetterOf(nameof(Text));

            SenderFileNameGetMethod = members.GetSourcePropertyGetterOf(nameof(SenderFileName));
            SenderFileNameSetMethod = members.GetSourcePropertySetterOf(nameof(SenderFileName));

            LineIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(LineIndex));
            LineIndexSetMethod = members.GetSourcePropertySetterOf(nameof(LineIndex));

            CharIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CharIndex));
            CharIndexSetMethod = members.GetSourcePropertySetterOf(nameof(CharIndex));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="LoadError"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected LoadError(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="LoadError"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static LoadError FromSource(object src) => src is null ? null : new LoadError(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// エラーの内容を指定して <see cref="LoadError"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="text">エラーの内容を表すテキスト。</param>
        /// <param name="senderFileName">エラーの発生元となるファイルのファイル名。</param>
        /// <param name="lineIndex">エラーの発生元となる行番号。</param>
        /// <param name="charIndex">エラーの発生元となる列番号。</param>
        public LoadError(string text, string senderFileName, int lineIndex, int charIndex) : this(Constructor.Invoke(new object[] { text, senderFileName, lineIndex, charIndex }))
        {
        }

        private static FastMethod TextGetMethod;
        private static FastMethod TextSetMethod;
        /// <summary>
        /// エラーの内容を表すテキストを取得します。
        /// </summary>
        public string Text
        {
            get => TextGetMethod.Invoke(Src, new object[0]);
            internal set => TextSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SenderFileNameGetMethod;
        private static FastMethod SenderFileNameSetMethod;
        /// <summary>
        /// エラーの発生元となるファイルのファイル名を取得します。
        /// </summary>
        public string SenderFileName
        {
            get => SenderFileNameGetMethod.Invoke(Src, new object[0]);
            internal set => SenderFileNameSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LineIndexGetMethod;
        private static FastMethod LineIndexSetMethod;
        /// <summary>
        /// エラーの発生元となる行番号を取得します。
        /// </summary>
        public int LineIndex
        {
            get => LineIndexGetMethod.Invoke(Src, new object[0]);
            internal set => LineIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CharIndexGetMethod;
        private static FastMethod CharIndexSetMethod;
        /// <summary>
        /// エラーの発生元となる列番号を取得します。
        /// </summary>
        public int CharIndex
        {
            get => CharIndexGetMethod.Invoke(Src, new object[0]);
            internal set => CharIndexSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
