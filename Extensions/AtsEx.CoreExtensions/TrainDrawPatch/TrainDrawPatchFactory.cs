﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;

using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.TrainDrawPatch
{
    [ExtensionMainDisplayType(typeof(ITrainDrawPatchFactory))]
    internal sealed class TrainDrawPatchFactory : AssemblyPluginBase, ITrainDrawPatchFactory
    {
        private readonly FastMethod DrawCarsMethod;

        public override string Title { get; } = nameof(TrainDrawPatch);
        public override string Description { get; } = "他列車を自由に移動・回転できるようにするパッチを提供します。";

        public TrainDrawPatchFactory(PluginBuilder builder) : base(builder, PluginType.Extension)
        {
            ClassMemberSet members = BveHacker.BveTypes.GetClassInfoOf<Train>();
            DrawCarsMethod = members.GetSourceMethodOf(nameof(Train.DrawCars));
        }

        public override void Dispose()
        {
        }

        public override TickResult Tick(TimeSpan elapsed) => new ExtensionTickResult();

        public TrainDrawPatch Patch(Train target, IMatrixConverter worldMatrixConverter)
            => new TrainDrawPatch(DrawCarsMethod, target, worldMatrixConverter, new DefaultMatrixConverter());

        public TrainDrawPatch Patch(Train target, IMatrixConverter worldMatrixConverter, IMatrixConverter viewMatrixConverter)
            => new TrainDrawPatch(DrawCarsMethod, target, worldMatrixConverter, viewMatrixConverter);

        public TrainDrawPatch Patch(Train target, Action<Direct3DProvider, Matrix> overrideAction)
            => new TrainDrawPatch(DrawCarsMethod, target, overrideAction);


        private class DefaultMatrixConverter : IMatrixConverter
        {
            public Matrix Convert(Matrix source) => source;
        }
    }
}
