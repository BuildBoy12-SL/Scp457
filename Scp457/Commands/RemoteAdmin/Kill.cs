// -----------------------------------------------------------------------
// <copyright file="Kill.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Commands.RemoteAdmin
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Command to kill all Scp457s.
    /// </summary>
    public class Kill : ICommand
    {
        private const string RequiredPermission = "457.kill";

        /// <inheritdoc />
        public string Command { get; } = "kill";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "k" };

        /// <inheritdoc />
        public string Description { get; } = "Kills all active Scp457s.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = $"Insufficient permission. Required: {RequiredPermission}";
                return false;
            }

            foreach (Player player in API.Scp457.List.Select(scp457 => scp457.Player))
                player.Kill();

            response = "Killed all Scp457s successfully.";
            return true;
        }
    }
}