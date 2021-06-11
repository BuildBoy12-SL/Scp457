// -----------------------------------------------------------------------
// <copyright file="Scp0492AttackPatches.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Patches
{
#pragma warning disable SA1118
#pragma warning disable SA1402
#pragma warning disable SA1649
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using Scp457.API;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Handles building new instructions.
    /// </summary>
    internal static class InstructionBuilder
    {
        /// <summary>
        /// Builds new instructions for <see cref="Scp049_2PlayerScript"/> attack patches to handle <see cref="Scp457"/>'s attack.
        /// </summary>
        /// <param name="instructions"><inheritdoc cref="CodeInstruction"/></param>
        /// <param name="generator"><inheritdoc cref="ILGenerator"/></param>
        /// <returns>New <see cref="CodeInstruction"/>.</returns>
        internal static IEnumerable<CodeInstruction> AttackInstructions(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var baseLogicLabel = generator.DefineLabel();

            newInstructions[0].WithLabels(baseLogicLabel);

            var scp457 = generator.DeclareLocal(typeof(Scp457));

            newInstructions.InsertRange(0, new[]
            {
                // Scp457 scp457 = Scp457.Get(this.gameObject);
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Component), nameof(Component.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Scp457), nameof(Scp457.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Stloc_S, scp457.LocalIndex),

                // if (scp457 == null) goto base game
                new CodeInstruction(OpCodes.Ldloc_S, scp457.LocalIndex),
                new CodeInstruction(OpCodes.Brfalse_S, baseLogicLabel),

                // scp457.Attack();
                new CodeInstruction(OpCodes.Ldloc_S, scp457.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Scp457), nameof(Scp457.Attack))),

                // return;
                new CodeInstruction(OpCodes.Ret),
            });

            for (int i = 0; i < newInstructions.Count; i++)
                yield return newInstructions[i];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }

    /// <summary>
    /// Patches <see cref="Scp049_2PlayerScript.CallCmdHurtPlayer"/> to override when a <see cref="Scp457"/> attacks.
    /// </summary>
    [HarmonyPatch(typeof(Scp049_2PlayerScript), nameof(Scp049_2PlayerScript.CallCmdHurtPlayer))]
    internal static class HurtPlayerPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            => InstructionBuilder.AttackInstructions(instructions, generator);
    }

    /// <summary>
    /// Patches <see cref="Scp049_2PlayerScript.CallCmdShootAnim"/> to override when a <see cref="Scp457"/> attacks.
    /// </summary>
    [HarmonyPatch(typeof(Scp049_2PlayerScript), nameof(Scp049_2PlayerScript.CallCmdShootAnim))]
    internal static class ShootAnimPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            => InstructionBuilder.AttackInstructions(instructions, generator);
    }
}