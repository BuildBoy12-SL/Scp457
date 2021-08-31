// -----------------------------------------------------------------------
// <copyright file="AttackWindowPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Patches
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Scp457.API;

    /// <summary>
    /// Patches <see cref="Scp049_2PlayerScript.CallCmdAttackWindow"/> to prevent <see cref="Scp457"/> from hitting windows.
    /// </summary>
    [HarmonyPatch(typeof(Scp049_2PlayerScript), nameof(Scp049_2PlayerScript.CmdAttackWindow))]
    internal static class AttackWindowPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            => InstructionBuilder.Instructions(instructions, generator);
    }
}