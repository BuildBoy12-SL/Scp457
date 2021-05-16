// -----------------------------------------------------------------------
// <copyright file="Scp457.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.API
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Gets and manipulates data relating to Scp457s.
    /// </summary>
    public class Scp457
    {
        private const string SessionVariable = "IsScp457";
        private readonly CoroutineHandle updateBurn;
        private CoroutineHandle updateCooldown;
        private float combustionCooldown;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scp457"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        private Scp457(Player player)
        {
            Player = player;
            Scp0492PlayerScript = player.GameObject.GetComponent<Scp049_2PlayerScript>();
            updateBurn = Timing.RunCoroutine(UpdateBurn());
            Log.Debug($"Instantiated {Player.Nickname} as a Scp457.", Plugin.Instance.Config.ShowDebug);
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="Scp457"/>s.
        /// </summary>
        public static Dictionary<Player, Scp457> Dictionary { get; } = new Dictionary<Player, Scp457>();

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of all <see cref="Scp457"/>s.
        /// </summary>
        public static IEnumerable<Scp457> List => Dictionary.Values;

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing cached <see cref="Scp457"/>s and their <see cref="GameObject"/>s.
        /// </summary>
        public static Dictionary<GameObject, Scp457> GameObjectCache { get; } = new Dictionary<GameObject, Scp457>();

        /// <summary>
        /// Gets the attached <see cref="Player"/>.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Player"/>'s <see cref="Scp049_2PlayerScript"/>.
        /// </summary>
        public Scp049_2PlayerScript Scp0492PlayerScript { get; }

        /// <summary>
        /// Gets or sets the cooldown of the combustion ability.
        /// </summary>
        public float CombustCooldown
        {
            get => combustionCooldown;
            set
            {
                if (combustionCooldown == 0f && value > 0f)
                {
                    combustionCooldown = value;
                    updateCooldown = Timing.RunCoroutine(UpdateCooldown());
                    return;
                }

                combustionCooldown = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Scp457"/> instance from a <see cref="Player"/>.
        /// </summary>
        /// <param name="gameObject">The player to search.</param>
        /// <returns>The <see cref="Scp457"/> instance or null.</returns>
        public static Scp457 Get(GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            if (GameObjectCache.TryGetValue(gameObject, out Scp457 scp457))
                return scp457;

            foreach (Player player in Dictionary.Keys)
            {
                if (player.GameObject != gameObject)
                    continue;

                Scp457 found = Dictionary[player];
                GameObjectCache[gameObject] = found;
                return found;
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="Scp457"/> instance from a <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The player to search.</param>
        /// <returns>The <see cref="Scp457"/> instance or null.</returns>
        public static Scp457 Get(Player player)
        {
            if (player == null)
                return null;

            Dictionary.TryGetValue(player, out Scp457 burningHandler);
            return burningHandler;
        }

        /// <summary>
        /// Spawns a user as a Scp457.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to spawn in as a Scp457.</param>
        public static void Spawn(Player player)
        {
            if (player == null)
                return;

            Dictionary.Add(player, new Scp457(player));

            player.Role = RoleType.Scp0492;

            Config config = Plugin.Instance.Config;

            player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            player.CustomInfo = config.Scp457Settings.Badge;

            player.Health = player.MaxHealth = config.Scp457Settings.Health;

            Vector3 scale = config.Scp457Settings.Size.ToVector3();
            if (player.Scale != scale)
                player.Scale = scale;

            player.ShowHint(config.Scp457Settings.SpawnMessage, config.Scp457Settings.SpawnMessageDuration);
            player.SessionVariables.Add(SessionVariable, true);
        }

        /// <summary>
        /// Removes the <see cref="Player"/> from being considered as Scp457.
        /// </summary>
        public void Destroy()
        {
            Timing.KillCoroutines(updateBurn);
            Timing.KillCoroutines(updateCooldown);
            Player.SessionVariables.Remove(SessionVariable);
            Dictionary.Remove(Player);
            Player.Scale = Vector3.one;
            Player.CustomInfo = string.Empty;
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            Log.Debug($"Destroyed {Player.Nickname}'s Scp457 instance.", Plugin.Instance.Config.ShowDebug);
        }

        /// <summary>
        /// Handles the attack logic for the <see cref="Scp457"/>.
        /// </summary>
        public void TryAttack()
        {
            Vector3 forward = Player.CameraTransform.forward;
            Ray ray = new Ray(Player.CameraTransform.position + forward, forward);

            WeaponManager weaponManager = Player.ReferenceHub.weaponManager;

            if (!Physics.Raycast(ray, out RaycastHit raycastHit, Plugin.Instance.Config.AttackSettings.Distance, weaponManager.raycastMask))
                return;

            Player target = Player.Get(raycastHit.collider.GetComponentInParent<NetworkIdentity>().gameObject);
            if (target == null)
            {
                PlaceBlood(Player, raycastHit.point);
                return;
            }

            if (target.IsScp || target.SessionVariables.ContainsKey("IsScp035")
                             || target.SessionVariables.ContainsKey("IsGhostSpectator")
                             || target.SessionVariables.ContainsKey("IsNPC"))
            {
                PlaceBlood(Player, target.Position);
                return;
            }

            if (weaponManager.GetShootPermission(target.ReferenceHub.characterClassManager))
            {
                RunAttack(target);
                PlaceBlood(Player, target.Position);
                return;
            }

            PlaceBlood(Player, raycastHit.point);
        }

        private static void PlaceBlood(Player attacker, Vector3 position) =>
            attacker.ReferenceHub.characterClassManager.RpcPlaceBlood(position, 1, 2);

        private void RunAttack(Player target)
        {
            if (!(BurningHandler.Get(target) is BurningHandler burningHandler))
                return;

            Config config = Plugin.Instance.Config;
            float burnTime = burningHandler.BurnTime + config.AttackSettings.BurnDuration;
            if (burnTime > Plugin.Instance.Config.BurnSettings.MaximumDuration)
                burnTime = Plugin.Instance.Config.BurnSettings.MaximumDuration;

            burningHandler.LastAttacker = this;
            burningHandler.BurnTime = burnTime;
            target.Hurt(config.AttackSettings.Damage, DamageTypes.Asphyxiation, Player.Nickname, Player.Id);
            Scp0492PlayerScript.TargetHitMarker(Player.Connection);
        }

        private IEnumerator<float> UpdateBurn()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);

                foreach (Player player in Player.List)
                {
                    if (!player.IsAlive || player.IsScp
                                        || player.SessionVariables.ContainsKey("IsScp035")
                                        || player.SessionVariables.ContainsKey("IsGhostSpectator")
                                        || player.SessionVariables.ContainsKey("IsNPC"))
                        continue;

                    if (!(BurningHandler.Get(player) is BurningHandler burningHandler))
                        continue;

                    if (Vector3.Distance(player.Position, Player.Position) > Plugin.Instance.Config.Scp457Settings.BurnRadius)
                    {
                        burningHandler.HasBurned = false;
                        continue;
                    }

                    if (Physics.Linecast(Player.Position, player.Position, player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                    {
                        burningHandler.HasBurned = false;
                        continue;
                    }

                    player.EnableEffect(EffectType.Burned, 0.2f);
                    burningHandler.HasBurned = true;
                }
            }
        }

        private IEnumerator<float> UpdateCooldown()
        {
            while (CombustCooldown > 0f)
            {
                yield return Timing.WaitForSeconds(1f);
                CombustCooldown--;
            }

            CombustCooldown = 0f;
        }
    }
}