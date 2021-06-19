// -----------------------------------------------------------------------
// <copyright file="InstructionBuilder.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Handles building new instructions.
    /// </summary>
    internal static class InstructionBuilder
    {
        /// <summary>
        /// Builds new instructions to prevent an action if the player is a Scp457.
        /// </summary>
        /// <param name="instructions"><inheritdoc cref="CodeInstruction"/></param>
        /// <param name="generator"><inheritdoc cref="ILGenerator"/></param>
        /// <returns>New <see cref="CodeInstruction"/>.</returns>
        internal static IEnumerable<CodeInstruction> Instructions(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var returnLabel = generator.DefineLabel();

            newInstructions[newInstructions.Count - 1].WithLabels(returnLabel);

            newInstructions.InsertRange(0, new[]
            {
                // if (Scp457.Get(this.gameObject) != null) return;
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Component), nameof(Component.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Scp457), nameof(API.Scp457.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Brtrue_S, returnLabel),
            });

            for (int i = 0; i < newInstructions.Count; i++)
                yield return newInstructions[i];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}