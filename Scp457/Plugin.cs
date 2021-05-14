// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457
{
    using System;
    using Exiled.API.Features;
    using Scp457.EventHandlers;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin InstanceValue = new Plugin();

        private Plugin()
        {
        }

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; } = InstanceValue;

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);

        /// <summary>
        /// Gets an instance of <see cref="Scp457.EventHandlers.MapEvents"/>.
        /// </summary>
        public MapEvents MapEvents { get; private set; }

        /// <summary>
        /// Gets an instance of <see cref="Scp457.EventHandlers.PlayerEvents"/>.
        /// </summary>
        public PlayerEvents PlayerEvents { get; private set; }

        /// <summary>
        /// Gets an instance of <see cref="Scp457.EventHandlers.ServerEvents"/>.
        /// </summary>
        public ServerEvents ServerEvents { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            MapEvents = new MapEvents();
            PlayerEvents = new PlayerEvents();
            ServerEvents = new ServerEvents();

            MapEvents.SubscribeEvents();
            PlayerEvents.SubscribeEvents();
            ServerEvents.SubscribeEvents();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            MapEvents.UnsubscribeEvents();
            PlayerEvents.UnsubscribeEvents();
            ServerEvents.UnsubscribeEvents();

            MapEvents = null;
            PlayerEvents = null;
            ServerEvents = null;

            base.OnDisabled();
        }
    }
}