// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457
{
    using Exiled.API.Features;

    /// <summary>
    /// Various extension methods for cleanliness.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns a value indicating whether the player is considered to be a ghost spectator.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>Whether the player is considered to be a ghost spectator.</returns>
        public static bool IsGhostSpectator(this Player player) => player.SessionVariables.ContainsKey("IsGhostSpectator");

        /// <summary>
        /// Returns a value indicating whether the player is considered to be a npc.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>Whether the player is considered to be a npc.</returns>
        public static bool IsNpc(this Player player) => player.SessionVariables.ContainsKey("IsNPC");

        /// <summary>
        /// Returns a value indicating whether the player is considered to be a Scp035.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>Whether the player is considered to be a Scp035.</returns>
        public static bool IsScp035(this Player player) => player.SessionVariables.ContainsKey("IsScp035");
    }
}