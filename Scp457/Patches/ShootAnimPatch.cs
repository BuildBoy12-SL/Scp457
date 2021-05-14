// -----------------------------------------------------------------------
// <copyright file="ShootAnimPatch.cs" company="Build">
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
    /// Patches <see cref="Scp049_2PlayerScript.CallCmdShootAnim"/> to override when a <see cref="Scp457"/> attacks.
    /// </summary>
    [HarmonyPatch(typeof(Scp049_2PlayerScript), nameof(Scp049_2PlayerScript.CallCmdShootAnim))]
    internal static class ShootAnimPatch
    {
        private static bool Prefix(Scp049_2PlayerScript __instance)
        {
            if (!(Scp457.Get(__instance.gameObject) is Scp457 scp457))
                return true;

            scp457.TryAttack();
            return false;
        }
    }
}