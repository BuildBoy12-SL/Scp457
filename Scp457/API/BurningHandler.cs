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
    using MEC;

    /// <summary>
    /// Gets and manipulates data relating to the applied burning effect.
    /// </summary>
    public class BurningHandler
    {
        private CoroutineHandle coroutine;
        private float burnTime;

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

        /// <summary>
        /// Gets or sets the remaining time for the player to burn.
        /// </summary>
        public float BurnTime
        {
            get => burnTime;
            set
            {
                if (burnTime == 0f && value > 0f)
                    coroutine = Timing.RunCoroutine(Burn());

                burnTime = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="BurningHandler"/> instance from a <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The player to search.</param>
        /// <returns>The <see cref="BurningHandler"/> instance or null.</returns>
        public static BurningHandler Get(Player player)
        {
            if (player == null)
                return null;

            Dictionary.TryGetValue(player, out BurningHandler burningHandler);
            return burningHandler;
        }

        /// <summary>
        /// Removes the <see cref="Player"/> from the handler.
        /// </summary>
        public void Destroy()
        {
            Timing.KillCoroutines(coroutine);
            Dictionary.Remove(Player);
        }

        /// <summary>
        /// Deals burn damage to the <see cref="Player"/> while their <see cref="BurnTime"/> is greater than 0.
        /// </summary>
        /// <returns>An internal delay.</returns>
        private IEnumerator<float> Burn()
        {
            while (BurnTime > 0f)
            {
                if (Player.IsGodModeEnabled)
                {
                    BurnTime = 0f;
                    yield break;
                }

                Player.Hurt(Plugin.Instance.Config.BurnSettings.Damage, DamageTypes.Asphyxiation, "SCP457");
                BurnTime -= Plugin.Instance.Config.BurnSettings.TickDuration;
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.BurnSettings.TickDuration);
            }

            BurnTime = 0f;
        }
    }
}