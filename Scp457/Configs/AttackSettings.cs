// -----------------------------------------------------------------------
// <copyright file="AttackSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Configs
{
    /// <summary>
    /// A set of configs to determine how Scp457s main attack interacts.
    /// </summary>
    public class AttackSettings
    {
        /// <summary>
        /// Gets or sets the amount of inflicted damage.
        /// </summary>
        public float Damage { get; set; } = 40f;

        /// <summary>
        /// Gets or sets the maximum attack range.
        /// </summary>
        public float Distance { get; set; } = 150f;

        /// <summary>
        /// Gets or sets the duration, in seconds, of the applied burning effect.
        /// </summary>
        public float BurnDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets a value indicating whether a line should be drawn when Scp457 attacks.
        /// </summary>
        public bool ShowAttack { get; set; } = true;

        /// <summary>
        /// Gets or sets the spacing of the drawn markers.
        /// </summary>
        public float OrbSpacing { get; set; } = 1f;
    }
}