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
    public class ServerEvents
    {
        /// <summary>
        /// Handles all subscriptions.
        /// </summary>
        public void SubscribeEvents()
        {
            ServerHandlers.ReloadedConfigs += OnReloadedConfigs;
            ServerHandlers.RoundStarted += OnRoundStarted;
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <summary>
        /// Handles all unsubscribing.
        /// </summary>
        public void UnsubscribeEvents()
        {
            ServerHandlers.ReloadedConfigs -= OnReloadedConfigs;
            ServerHandlers.RoundStarted -= OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnReloadedConfigs()
        {
            if (Plugin.Instance.Config.AttackSettings.OrbSpacing <= 0f)
            {
                Plugin.Instance.Config.AttackSettings.OrbSpacing = 0.5f;
                Log.Warn("Do not set the Orb Spacing to 0 or less than 0! Reverted to default value.");
            }
        }

        private void OnRoundStarted()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                if (Plugin.Instance.Config.Scp457Settings.SpawnChance <= Random.Range(0, 100))
                    return;

                List<Player> players = Player.List.Where(x => x.Team == Team.SCP).ToList();
                if (players.Count == 0)
                    return;

                Player player = players[Random.Range(0, players.Count)];
                Scp457.Spawn(player);
            });
        }

        private void OnWaitingForPlayers()
        {
            Scp457.Dictionary.Clear();
            BurningHandler.Dictionary.Clear();
        }
    }
}