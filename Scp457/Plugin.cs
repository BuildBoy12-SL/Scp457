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
    using HarmonyLib;
    using Scp457.EventHandlers;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin InstanceValue = new Plugin();
        private Harmony harmony;

        private Plugin()
        {
        }

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; } = InstanceValue;

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            MapEvents.SubscribeEvents();
            PlayerEvents.SubscribeEvents();
            ServerEvents.SubscribeEvents();

            harmony = new Harmony($"build.scp457.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            MapEvents.UnsubscribeEvents();
            PlayerEvents.UnsubscribeEvents();
            ServerEvents.UnsubscribeEvents();

            harmony.UnpatchAll();

            base.OnDisabled();
        }
    }
}