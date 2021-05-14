// -----------------------------------------------------------------------
// <copyright file="CombustSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Configs
{
    /// <summary>
    /// A set of configs to determine how Scp457s combustion interacts.
    /// </summary>
    public class CombustSettings
    {
        /// <summary>
        /// Gets or sets the amount of seconds between uses.
        /// </summary>
        public float Cooldown { get; set; } = 30f;

        /// <summary>
        /// Gets or sets the initial damage of the explosion.
        /// </summary>
        public float Damage { get; set; } = 15f;

        /// <summary>
        /// Gets or sets the duration of the applied burning effect.
        /// </summary>
        public float BurnDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the duration of the applied <see cref="CustomPlayerEffects.Flashed"/> effect.
        /// </summary>
        public float FlashDuration { get; set; }

        /// <summary>
        /// Gets or sets the message to be sent to a user who is on cooldown.
        /// </summary>
        public string CooldownMessage { get; set; } = "<color=red>Wait %seconds% to use that command again.</color>";

        /// <summary>
        /// Gets or sets the message to be sent when combustion executes successfully.
        /// </summary>
        public string UsedMessage { get; set; } = "<color=green>Done.</color>";
    }
}