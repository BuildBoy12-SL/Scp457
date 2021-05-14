// -----------------------------------------------------------------------
// <copyright file="BurningHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.API
{
    using System.Collections.Generic;
    using Exiled.API.Features;

    /// <summary>
    /// Gets and manipulates data relating to the applied burning effect.
    /// </summary>
    public class BurningHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BurningHandler"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        public BurningHandler(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="BurningHandler"/>s.
        /// </summary>
        public static Dictionary<Player, BurningHandler> Dictionary { get; } = new Dictionary<Player, BurningHandler>();

        /// <summary>
        /// Gets a list of all <see cref="BurningHandler"/>s.
        /// </summary>
        public static IEnumerable<BurningHandler> List => Dictionary.Values;

        /// <summary>
        /// Gets the attached <see cref="Player"/>.
        /// </summary>
        public Player Player { get; }

        public static BurningHandler Get(Player player)
        {
            if (player == null)
                return null;

            Dictionary.TryGetValue(player, out BurningHandler burningHandler);
            return burningHandler;
        }
    }
}