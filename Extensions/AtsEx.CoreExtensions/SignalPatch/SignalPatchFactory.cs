﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;

using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.SignalPatch
{
    [PluginType(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(ISignalPatchFactory))]
    internal sealed class SignalPatchFactory : AssemblyPluginBase, ISignalPatchFactory
    {
        private readonly FastMethod GetCurrentSignalIndexMethod;

        public override string Title { get; } = nameof(SignalPatch);
        public override string Description { get; } = "閉塞の信号現示を自由に変更できるようにするパッチを提供します。";

        public SignalPatchFactory(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet members = BveHacker.BveTypes.GetClassInfoOf<Section>();
            GetCurrentSignalIndexMethod = members.GetSourcePropertyGetterOf(nameof(Section.CurrentSignalIndex));
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            BveHacker.Scenario.SectionManager.OnSignalChanged();
            return new ExtensionTickResult();
        }

        public SignalPatch Patch(string name, Section target, Converter<int, int> factory)
            => new SignalPatch(name, GetCurrentSignalIndexMethod, target, factory);

        public SignalPatch Patch(string name, SectionManager _, Section target, Converter<int, int> factory) => Patch(name, target, factory);
    }
}
