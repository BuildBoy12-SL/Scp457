// -----------------------------------------------------------------------
// <copyright file="AttackSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.Configs
{
    using Exiled.API.Features;

    /// <summary>
    /// A set of configs to determine how Scp457s main attack interacts.
    /// </summary>
    public class AttackSettings
    {
        private float orbSpacing = 1f;

        /// <summary>
        /// Gets or sets the amount of inflicted damage.
        /// </summary>
        public float Damage { get; set; } = 40f;

        /// <summary>
        /// Gets or sets the maximum attack range.
        /// </summary>
        public float Distance { get; set; } = 150f;

        /// <summary>
        /// Gets or sets a value indicating whether the attack can hit multiple targets at once.
        /// </summary>
        public bool Pierce { get; set; } = true;

        /// <summary>
        /// Gets or sets the duration, in seconds, of the applied burning effect.
        /// </summary>
        public float BurnDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets a value indicating whether blood should be placed where an attack lands.
        /// </summary>
        public bool PlaceBlood { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a line should be drawn when Scp457 attacks.
        /// </summary>
        public bool ShowAttack { get; set; } = true;

        /// <summary>
        /// Gets or sets the spacing of the drawn markers.
        /// </summary>
        public float OrbSpacing
        {
            get => orbSpacing;
            set
            {
                if (value <= 0f)
                {
                    value = 1f;
                    Log.Warn("Do not set the Orb Spacing config to 0 or less than 0! Reverted to the default value of 1.");
                }

                orbSpacing = value;
            }
        }
    }
}