// -----------------------------------------------------------------------
// <copyright file="AttackWindowPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using Scp457.API;

    /// <summary>
    /// Patches <see cref="Scp049_2PlayerScript.CallCmdAttackWindow"/> to prevent <see cref="Scp457"/> from hitting windows.
    /// </summary>
    [HarmonyPatch(typeof(Scp049_2PlayerScript), nameof(Scp049_2PlayerScript.CallCmdAttackWindow))]
    internal static class AttackWindowPatch
    {
        private static bool Prefix(Scp049_2PlayerScript __instance)
        {
            return Scp457.Get(__instance.gameObject) == null;
        }
    }
}