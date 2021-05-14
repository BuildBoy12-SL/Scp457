// -----------------------------------------------------------------------
// <copyright file="Scp457Settings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Configs
{
    using Scp457.API;

    /// <summary>
    /// A set of configs to determine how Scp457 generally interacts.
    /// </summary>
    public class Scp457Settings
    {
        /// <summary>
        /// Gets or sets the base health of the Scp.
        /// </summary>
        public int Health { get; set; } = 1200;

        /// <summary>
        /// Gets or sets the percentage chance that Scp457 will spawn in place of another Scp.
        /// </summary>
        public float SpawnChance { get; set; } = 20;

        /// <summary>
        /// Gets or sets the <see cref="Exiled.API.Features.Player.Scale"/> of Scp457.
        /// </summary>
        public Vector Size { get; set; } = new Vector
        {
            X = 1.1f,
            Y = 1.15f,
            Z = 1.1f,
        };

        /// <summary>
        /// Gets or sets the name of the room that the Scp will spawn in.
        /// </summary>
        public string SpawnRoom { get; set; } = "HCZ_ARMORY";

        /// <summary>
        /// Gets or sets the radius around Scp457 where players who have line of sight.
        /// </summary>
        public float BurnRadius { get; set; }

        /// <summary>
        /// Gets or sets the message to be displayed to a Scp457 when they spawn.
        /// </summary>
        public string SpawnMessage { get; set; } = "You have spawned as <color=red>SCP-457</color>\nKill everyone.";

        /// <summary>
        /// Gets or sets the amount of time that the <see cref="SpawnMessage"/> is displayed.
        /// </summary>
        public ushort SpawnMessageDuration { get; set; } = 10;

        /// <summary>
        /// Gets or sets the message to be shown where Scp457s role would normally be.
        /// </summary>
        public string Badge { get; set; } = "<color=#990000>Scp-457</color>";
    }
}