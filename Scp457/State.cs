// -----------------------------------------------------------------------
// <copyright file="State.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Used for tracking across the project.
    /// </summary>
    public static class State
    {
        /// <summary>
        /// Gets a collection of grenades which explosions should not do anything.
        /// </summary>
        public static List<GameObject> IgnoredGrenades { get; } = new List<GameObject>();
    }
}