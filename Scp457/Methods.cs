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
    using Exiled.API.Extensions;
    using Exiled.API.Features.Items;
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

        public static ExplosiveGrenade SpawnGrenade(Vector3 position)
        {
            var grenade = new ExplosiveGrenade(GrenadeType.FragGrenade.GetItemType())
            {
                FuseTime = 0.1f,
            };

            grenade.SpawnActive(position);
            IgnoredGrenades.Add(grenade.Base.gameObject);
            return grenade;
        }
    }
}