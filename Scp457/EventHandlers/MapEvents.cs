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
    public class MapEvents
    {
        /// <summary>
        /// Handles all subscriptions.
        /// </summary>
        public void SubscribeEvents()
        {
            MapHandlers.ExplodingGrenade += OnExplodingGrenade;
        }

        /// <summary>
        /// Handles all unsubscribing.
        /// </summary>
        public void UnsubscribeEvents()
        {
            MapHandlers.ExplodingGrenade -= OnExplodingGrenade;
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (State.IgnoredGrenades.Remove(ev.Grenade))
                ev.IsAllowed = false;
        }
    }
}