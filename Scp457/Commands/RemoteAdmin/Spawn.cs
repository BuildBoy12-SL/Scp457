// -----------------------------------------------------------------------
// <copyright file="Spawn.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Commands.RemoteAdmin
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using global::RemoteAdmin;
    using Scp457.API;

    /// <summary>
    /// Command to spawn a Scp457.
    /// </summary>
    public class Spawn : ICommand
    {
        private const string RequiredPermission = "457.spawn";

        /// <inheritdoc />
        public string Command { get; } = "spawn";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc />
        public string Description { get; } = "Spawns a user as a Scp457.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = $"Insufficient permission. Required: {RequiredPermission}";
                return false;
            }

            Player player = Player.Get((sender as PlayerCommandSender)?.ReferenceHub);
            if (arguments.Count > 0)
            {
                if (!(Player.Get(arguments.At(0)) is Player ply))
                {
                    response = "Could not find the referenced user.";
                    return false;
                }

                player = ply;
            }

            if (Scp457.Get(player) != null)
            {
                response = $"{player.Nickname} is already a Scp457!";
                return false;
            }

            Scp457.Spawn(player);
            response = $"Spawned {player.Nickname} as a Scp457.";
            return true;
        }
    }
}