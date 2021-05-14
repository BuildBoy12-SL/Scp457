// -----------------------------------------------------------------------
// <copyright file="List.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Commands.RemoteAdmin
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Command to list all Scp457s.
    /// </summary>
    public class List : ICommand
    {
        private const string RequiredPermission = "457.list";

        /// <inheritdoc />
        public string Command { get; } = "list";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "l" };

        /// <inheritdoc />
        public string Description { get; } = "Lists all active Scp457s.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = $"Insufficient permission. Required: {RequiredPermission}";
                return false;
            }

            response = $"Alive Scp457s: {string.Join(", ", API.Scp457.List.Select(scp457 => scp457.Player.Nickname))}";
            return true;
        }
    }
}