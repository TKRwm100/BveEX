﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.SignalPatch
{
    /// <summary>
    /// <see cref="SignalPatch"/> クラスの新しいインスタンスを初期化するたのファクトリを表します。
    /// </summary>
    public interface ISignalPatchFactory : IExtension
    {
        /// <summary>
        /// 指定した閉塞へ、信号現示を自由に変更できるようにするパッチを適用します。
        /// </summary>
        /// <param name="name">パッチの名前。</param>
        /// <param name="sectionManager">信号現示の変更に使用する <see cref="SectionManager"/>。</param>
        /// <param name="target">パッチを適用する閉塞。</param>
        /// <param name="factory"><see cref="Section.CurrentSignalIndex"/> プロパティの処理をオーバーライドするデリゲート。<see langword="null"/> を返すと本来の値が使用されます。</param>
        /// <returns>パッチを表す <see cref="SignalPatch"/>。</returns>
        SignalPatch Patch(string name, SectionManager sectionManager, Section target, Converter<int, int> factory);
    }
}
