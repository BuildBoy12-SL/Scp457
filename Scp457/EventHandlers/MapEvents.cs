// -----------------------------------------------------------------------
// <copyright file="MapEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.EventHandlers
{
    using Exiled.Events.EventArgs;
    using MapHandlers = Exiled.Events.Handlers.Map;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Map"/>.
    /// </summary>
    public static class MapEvents
    {
        /// <summary>
        /// Handles all subscriptions.
        /// </summary>
        public static void SubscribeEvents()
        {
            MapHandlers.ExplodingGrenade += OnExplodingGrenade;
        }

        /// <summary>
        /// Handles all unsubscribing.
        /// </summary>
        public static void UnsubscribeEvents()
        {
            MapHandlers.ExplodingGrenade -= OnExplodingGrenade;
        }

        private static void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (Methods.IgnoredGrenades.Remove(ev.Grenade.gameObject))
                ev.IsAllowed = false;
        }
    }
}