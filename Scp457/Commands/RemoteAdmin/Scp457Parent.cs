// -----------------------------------------------------------------------
// <copyright file="Scp457Parent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Commands.RemoteAdmin
{
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Parent command for all Scp457 remote admin commands.
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Scp457Parent : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Scp457Parent"/> class.
        /// </summary>
        public Scp457Parent() => LoadGeneratedCommands();

        /// <inheritdoc />
        public override string Command { get; } = "scp457";

        /// <inheritdoc />
        public override string[] Aliases { get; } = { "457" };

        /// <inheritdoc />
        public override string Description { get; } = "Parent command for Scp457.";

        /// <inheritdoc />
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Kill());
            RegisterCommand(new List());
            RegisterCommand(new Spawn());
        }

        /// <inheritdoc />
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases.Length > 0
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return false;
        }
    }
}