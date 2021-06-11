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
        private readonly Config config;
        private CoroutineHandle burn;
        private float burnTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="BurningHandler"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        public BurningHandler(Player player)
        {
            Player = player;
            config = Plugin.Instance.Config;
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
        /// Gets or sets the Scp457 that attacked the <see cref="Player"/> last.
        /// </summary>
        public Scp457 LastAttacker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Player"/> is under the line of sight <see cref="CustomPlayerEffects.Burned"/> effect.
        /// </summary>
        public bool HasBurned { get; set; }

        /// <summary>
        /// Gets or sets the remaining time for the player to burn.
        /// </summary>
        public float BurnTime
        {
            get => burnTime;
            set
            {
                if (burnTime <= 0f && value > 0f)
                {
                    burnTime = value;
                    burn = Timing.RunCoroutine(Burn());
                    return;
                }

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
            Timing.KillCoroutines(burn);
            Dictionary.Remove(Player);
        }

        /// <summary>
        /// Deals burn damage to the <see cref="Player"/> while their <see cref="BurnTime"/> is greater than 0.
        /// </summary>
        /// <returns>An internal delay.</returns>
        private IEnumerator<float> Burn()
        {
            Log.Debug($"Starting burn sequence for {Player.Nickname}.", config.ShowDebug);
            while (BurnTime > 0f)
            {
                if (Player.IsGodModeEnabled || LastAttacker == null
                                            || LastAttacker.Scp0492PlayerScript == null
                                            || LastAttacker.Player == null)
                {
                    Log.Debug($"{Player.Nickname} is in god mode or required logic is null, ending burn sequence.", config.ShowDebug);
                    break;
                }

                Player.Hurt(config.BurnSettings.Damage, DamageTypes.Asphyxiation, LastAttacker.Player.Nickname, LastAttacker.Player.Id);
                LastAttacker.Scp0492PlayerScript.TargetHitMarker(LastAttacker.Player.Connection);
                BurnTime -= config.BurnSettings.TickDuration;
                Log.Debug($"Damaged {Player.Nickname} on burn, waiting for tick duration.", config.ShowDebug);
                yield return Timing.WaitForSeconds(config.BurnSettings.TickDuration);
            }

            BurnTime = 0f;
            Log.Debug($"Ended burn sequence for {Player.Nickname}.", config.ShowDebug);
        }
    }
}