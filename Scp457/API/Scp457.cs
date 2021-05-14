// -----------------------------------------------------------------------
// <copyright file="Scp457.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.API
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Gets and manipulates data relating to Scp457s.
    /// </summary>
    public class Scp457
    {
        private const string SessionVariable = "IsScp457";

        /// <summary>
        /// Initializes a new instance of the <see cref="Scp457"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        private Scp457(Player player)
        {
            Player = player;
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
        /// Gets or sets the cooldown of the combustion ability.
        /// </summary>
        public float CombustCooldown { get; set; }

        /// <summary>
        /// Removes a player from being considered as Scp457.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be removed from being Scp457.</param>
        public static void Destroy(Player player)
        {
            if (Get(player) == null)
                return;

            player.SessionVariables.Remove(SessionVariable);
            Dictionary.Remove(player);
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
            Dictionary.Add(player, new Scp457(player));
        }

        /// <summary>
        /// Handles the attack logic for the <see cref="Scp457"/>.
        /// </summary>
        public void TryAttack()
        {
            Vector3 forward = Player.CameraTransform.forward;
            Ray ray = new Ray(Player.CameraTransform.position + forward, forward);

            WeaponManager weaponManager = Player.ReferenceHub.weaponManager;

            bool hit = Physics.Raycast(ray, out RaycastHit raycastHit, Plugin.Instance.Config.AttackSettings.Distance, weaponManager.raycastMask);

            if (!hit)
                return;

            Player target = Player.Get(raycastHit.collider.GetComponentInParent<NetworkIdentity>().gameObject);
            if (target == null || target.IsScp || target.SessionVariables.ContainsKey("IsScp035"))
                return;

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

            burningHandler.BurnTime = burnTime;
            target.Hurt(config.AttackSettings.Damage, DamageTypes.Asphyxiation, Player.Nickname, Player.Id);
        }
    }
}