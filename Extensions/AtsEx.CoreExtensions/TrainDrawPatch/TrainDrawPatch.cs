﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D9;

using BveTypes;
using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using TypeWrapping;

namespace AtsEx.Extensions.TrainDrawPatch
{
    /// <summary>
    /// 他列車を自由に移動・回転できるようにするパッチを表します。
    /// </summary>
    public sealed class TrainDrawPatch : IDisposable
    {
        private readonly Train Target;
        private readonly HarmonyPatch HarmonyPatch;

        internal TrainDrawPatch(FastMethod drawCarsMethod, Train target, Action<Direct3DProvider, Matrix> overrideAction)
        {
            Target = target;

            HarmonyPatch = HarmonyPatch.PatchAsync(drawCarsMethod.Source, PatchTypes.Prefix).Result;
            HarmonyPatch.Prefix += Prefix;


            PatchInvokationResult Prefix(object sender, PatchInvokedEventArgs e)
            {
                if (e.Instance != Target.Src) return new PatchInvokationResult();

                Direct3DProvider direct3DProvider = Direct3DProvider.FromSource(e.Args[0]);
                Matrix viewMatrix = (Matrix)e.Args[1];

                overrideAction(direct3DProvider, viewMatrix);

                return new PatchInvokationResult(true);
            }
        }

        internal TrainDrawPatch(FastMethod drawCarsMethod, Train target, IMatrixConverter worldMatrixConverter, IMatrixConverter viewMatrixConverter)
            : this(drawCarsMethod, target, (direct3DProvider, viewMatrix) => DrawCars(direct3DProvider, viewMatrix, target, worldMatrixConverter, viewMatrixConverter))
        {
        }

        private static void DrawCars(Direct3DProvider direct3DProvider, Matrix viewMatrix, Train target, IMatrixConverter worldMatrixConverter, IMatrixConverter viewMatrixConverter)
        {
            int distance = (int)Math.Floor(target.Location - target.UserVehicleLocationManager.Location);

            foreach (Structure structure in target.TrainInfo.Structures)
            {
                bool isTooFarInFront = distance + structure.Location >= target.DrawDistanceManager.FrontDrawDistance + 25.0;
                bool isTooFarInBack = distance + structure.Location + structure.Span < -target.DrawDistanceManager.BackDrawDistance - 25.0;

                if (!(structure.Model is null) && !isTooFarInFront && !isTooFarInBack)
                {
                    structure.TrackKey = target.TrainInfo.TrackKey;
                    Matrix trackMatrix = target.Route.GetTrackMatrix(structure, target.Location + structure.Location, (double)(target.UserVehicleLocationManager.BlockIndex * 25));
                    direct3DProvider.Device.SetTransform(TransformState.World, worldMatrixConverter.Convert(trackMatrix) * viewMatrixConverter.Convert(viewMatrix));
                    structure.Model.Draw(direct3DProvider, false);
                    structure.Model.Draw(direct3DProvider, true);
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            HarmonyPatch.Dispose();
        }
    }
}
