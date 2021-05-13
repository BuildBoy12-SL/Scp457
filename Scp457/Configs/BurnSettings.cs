// -----------------------------------------------------------------------
// <copyright file="BurnSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Configs
{
    /// <summary>
    /// A set of configs to determine how Scp457s burning interacts.
    /// </summary>
    public class BurnSettings
    {
        /// <summary>
        /// Gets or sets the amount of dealt damage per tick.
        /// </summary>
        public float Damage { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the seconds between each tick.
        /// </summary>
        public float TickDuration { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the maximum amount of seconds a user can be on fire.
        /// </summary>
        public float MaximumDuration { get; set; } = 20f;

        /// <summary>
        /// Gets or sets a collection of items that can heal a burn.
        /// </summary>
        public ItemType[] HealedBy { get; set; } =
        {
            ItemType.Medkit,
            ItemType.SCP500,
        };
    }
}