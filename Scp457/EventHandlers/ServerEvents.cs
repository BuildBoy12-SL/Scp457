// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.EventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using MEC;
    using Scp457.API;
    using UnityEngine;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public static class ServerEvents
    {
        /// <summary>
        /// Handles all subscriptions.
        /// </summary>
        public static void SubscribeEvents()
        {
            ServerHandlers.RoundStarted += OnRoundStarted;
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <summary>
        /// Handles all unsubscribing.
        /// </summary>
        public static void UnsubscribeEvents()
        {
            ServerHandlers.RoundStarted -= OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private static void OnRoundStarted()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                if (Plugin.Instance.Config.Scp457Settings.SpawnChance <= Random.Range(0, 100))
                    return;

                List<Player> players = Player.List.Where(ply => ply.Team == Team.SCP && !ply.IsNpc()).ToList();
                if (players.Count == 0)
                    return;

                Player player = players[Random.Range(0, players.Count)];
                Scp457.Spawn(player);
            });
        }

        private static void OnWaitingForPlayers()
        {
            Scp457.Dictionary.Clear();
            BurningHandler.Dictionary.Clear();
        }
    }
}