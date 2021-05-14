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
    using Scp457.API;

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
            if (!Scp457.IsScp457(player, out Scp457 scp457))
            {
                response = "You must be Scp457 to use this command.";
                return false;
            }

            

            response = "";
            return false;
        }
    }
}