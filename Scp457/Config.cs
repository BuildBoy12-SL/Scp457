// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457
{
    using Exiled.API.Interfaces;
    using Scp457.Configs;

    /// <inheritdoc cref="IConfig"/>
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages should be shown.
        /// </summary>
        public bool ShowDebug { get; set; } = true;

        /// <inheritdoc cref="Configs.AttackSettings"/>
        public AttackSettings AttackSettings { get; set; } = new AttackSettings();

        /// <inheritdoc cref="Configs.BurnSettings"/>
        public BurnSettings BurnSettings { get; set; } = new BurnSettings();

        /// <inheritdoc cref="Configs.CombustSettings"/>
        public CombustSettings CombustSettings { get; set; } = new CombustSettings();

        /// <inheritdoc cref="Configs.Scp457Settings"/>
        public Scp457Settings Scp457Settings { get; set; } = new Scp457Settings();
    }
}