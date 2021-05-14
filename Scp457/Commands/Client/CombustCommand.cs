// -----------------------------------------------------------------------
// <copyright file="CombustCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Commands.Client
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using global::RemoteAdmin;
    using Grenades;
    using Scp457.API;
    using UnityEngine;

    /// <summary>
    /// Command for Scp457 to use their combust ability.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CombustCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; } = "combust";

        /// <inheritdoc />
        public string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public string Description { get; } = "Triggers the Scp457 combustion ability.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((sender as PlayerCommandSender)?.SenderId);
            if (!(Scp457.Get(player) is Scp457 scp457))
            {
                response = "You must be Scp457 to use this command.";
                return false;
            }

            Config config = Plugin.Instance.Config;
            Grenade grenade = Methods.SpawnGrenade(player.Position, Vector3.zero, player, 0.1f);
            if (grenade != null)
                Methods.IgnoredGrenades.Add(grenade.gameObject);

            response = config.CombustSettings.UsedMessage;
            return false;
        }
    }
}