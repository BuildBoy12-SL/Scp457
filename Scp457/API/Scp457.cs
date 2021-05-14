// -----------------------------------------------------------------------
// <copyright file="Scp457.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.API
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using UnityEngine;

    /// <summary>
    /// Gets and manipulates data relating to Scp457s.
    /// </summary>
    public class Scp457
    {
        private const string SessionVariable = "IsScp457";

        /// <summary>
        /// Initializes a new instance of the <see cref="Scp457"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        public Scp457(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="Scp457"/>s.
        /// </summary>
        public static Dictionary<Player, Scp457> Dictionary { get; } = new Dictionary<Player, Scp457>();

        /// <summary>
        /// Gets a list of all <see cref="Scp457"/>s.
        /// </summary>
        public static IEnumerable<Scp457> List => Dictionary.Values;

        /// <summary>
        /// Gets the attached <see cref="Player"/>.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Removes a player from being considered as Scp457.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be removed from being Scp457.</param>
        public static void Destroy(Player player)
        {
            if (!IsScp457(player, out _))
                return;

            player.SessionVariables.Remove(SessionVariable);
        }

        /// <summary>
        /// Determines if a given <see cref="Player"/> is a Scp457.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check for being a Scp457.</param>
        /// <param name="scp457">The found <see cref="Scp457"/> instance.</param>
        /// <returns>A value indicating whether the <see cref="Player"/> is a Scp457.</returns>
        public static bool IsScp457(Player player, out Scp457 scp457)
        {
            if (player == null)
            {
                scp457 = null;
                return false;
            }

            return Dictionary.TryGetValue(player, out scp457);
        }

        /// <summary>
        /// Spawns a user as a Scp457.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to spawn in as a Scp457.</param>
        public static void Spawn(Player player)
        {
            if (player == null)
                return;

            player.Role = RoleType.Scp0492;

            Config config = Plugin.Instance.Config;

            player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            player.CustomInfo = config.Scp457Settings.Badge;

            player.Health = player.MaxHealth = config.Scp457Settings.Health;

            Vector3 scale = config.Scp457Settings.Size.ToVector3();
            if (player.Scale != scale)
                player.Scale = scale;

            player.ShowHint(config.Scp457Settings.SpawnMessage, config.Scp457Settings.SpawnMessageDuration);
            player.SessionVariables.Add(SessionVariable, true);
        }
    }
}