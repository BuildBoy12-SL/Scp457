// -----------------------------------------------------------------------
// <copyright file="Methods.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Grenades;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Used for tracking and abstraction across the project.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Gets a collection of grenades which explosions should not do anything.
        /// </summary>
        public static List<GameObject> IgnoredGrenades { get; } = new List<GameObject>();

        /// <summary>
        /// Spawns a live grenade object on the map.
        /// </summary>
        /// <param name="position">The <see cref="Vector3"/> to spawn the grenade at.</param>
        /// <param name="velocity">The <see cref="Vector3"/> directional velocity the grenade should move at.</param>
        /// /// <param name="player">The <see cref="Player"/> to count as the thrower of the grenade.</param>
        /// <param name="fuseTime">The <see cref="float"/> fuse time of the grenade.</param>
        /// <param name="grenadeType">The <see cref="GrenadeType"/> of the grenade to spawn.</param>
        /// <returns>The <see cref="Grenade"/> being spawned.</returns>
        public static Grenade SpawnGrenade(Vector3 position, Vector3 velocity, Player player, float fuseTime = 3f, GrenadeType grenadeType = GrenadeType.FragGrenade)
        {
            if (player == null)
                player = Server.Host;

            var component = player.GrenadeManager;
            var component2 = Object.Instantiate(component.availableGrenades[(int)grenadeType].grenadeInstance).GetComponent<Grenade>();

            component2.FullInitData(component, position, Quaternion.Euler(component2.throwStartAngle), velocity, component2.throwAngularVelocity, player == Server.Host ? Team.SCP : player.Team);
            component2.NetworkfuseTime = NetworkTime.time + fuseTime;
            NetworkServer.Spawn(component2.gameObject);

            return component2;
        }
    }
}