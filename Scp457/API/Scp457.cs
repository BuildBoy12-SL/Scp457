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
        private static readonly int WallMask = LayerMask.GetMask("Default", "Door", "Glass");
        private readonly Config config;
        private readonly List<Vector3> points = new List<Vector3>();
        private readonly RaycastHit[] hits = new RaycastHit[20];
        private readonly CoroutineHandle updateBurn;
        private CoroutineHandle updateCooldown;
        private int combustionCooldown;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scp457"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that this is to be attached to.</param>
        private Scp457(Player player)
        {
            Player = player;
            Scp0492PlayerScript = Player.GameObject.GetComponent<Scp049_2PlayerScript>();
            config = Plugin.Instance.Config;
            updateBurn = Timing.RunCoroutine(UpdateBurn());
            Log.Debug($"Instantiated {Player.Nickname} as a Scp457.", config.ShowDebug);
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
        public int CombustCooldown
        {
            get => combustionCooldown;
            set
            {
                if (combustionCooldown <= 0 && value > 0)
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

            Config config = Plugin.Instance.Config;

            Dictionary.Add(player, new Scp457(player));

            player.Role = RoleType.Scp0492;

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
            GameObjectCache.Remove(Player.GameObject);
            Player.Scale = Vector3.one;
            Player.CustomInfo = string.Empty;
            Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            Log.Debug($"Destroyed {Player.Nickname}'s Scp457 instance.", config.ShowDebug);
        }

        /// <summary>
        /// Handles the attack logic for the <see cref="Scp457"/>.
        /// </summary>
        public void Attack()
        {
            Vector3 forward = Player.CameraTransform.forward;
            Vector3 cameraPosition = Player.CameraTransform.position;

            Ray ray = new Ray(cameraPosition, forward);
            Vector3 endPoint = cameraPosition + (forward * config.AttackSettings.Distance);

            if (config.AttackSettings.Pierce)
            {
                TryPierceHit(ray, endPoint);
                return;
            }

            TryHit(ray, endPoint);
        }

        private static bool IsTargetable(Player player)
        {
            return player.IsAlive && !player.IsScp
                                  && !player.IsScp035()
                                  && !player.IsGhostSpectator()
                                  && !player.IsNpc();
        }

        private void TryHit(Ray ray, Vector3 endPoint)
        {
            bool hit = Physics.Raycast(ray, out var raycastHit, config.AttackSettings.Distance, Player.ReferenceHub.weaponManager.raycastMask);

            if (config.AttackSettings.ShowAttack)
                DrawAttack(hit ? raycastHit.point : endPoint);

            if (hit)
                TryAttack(raycastHit);
        }

        private void TryPierceHit(Ray ray, Vector3 endPoint)
        {
            float attackDistance = config.AttackSettings.Distance;
            if (config.AttackSettings.ShowAttack)
            {
                bool hit = Physics.Raycast(ray, out RaycastHit raycastHit, attackDistance, WallMask);
                DrawAttack(hit ? raycastHit.point : endPoint);
            }

            int hitCount = Physics.RaycastNonAlloc(ray, hits, attackDistance, Player.ReferenceHub.weaponManager.raycastMask);
            for (int i = 0; i < hitCount; i++)
                TryAttack(hits[i]);
        }

        private void TryAttack(RaycastHit raycastHit)
        {
            var netIdentity = raycastHit.collider.GetComponentInParent<NetworkIdentity>();
            if (netIdentity == null)
                return;

            Player target = Player.Get(netIdentity.gameObject);
            if (target == null || !IsTargetable(target))
                return;

            if (Player.ReferenceHub.weaponManager.GetShootPermission(target.ReferenceHub.characterClassManager))
                RunAttack(target);
        }

        private void RunAttack(Player target)
        {
            if (!(BurningHandler.Get(target) is BurningHandler burningHandler))
                return;

            float burnTime = burningHandler.BurnTime + config.AttackSettings.BurnDuration;
            if (burnTime > config.BurnSettings.MaximumDuration)
                burnTime = config.BurnSettings.MaximumDuration;

            burningHandler.LastAttacker = this;
            burningHandler.BurnTime = burnTime;
            target.Hurt(config.AttackSettings.Damage, DamageTypes.Asphyxiation, Player.Nickname, Player.Id);
            Scp0492PlayerScript.TargetHitMarker(Player.Connection);

            if (config.AttackSettings.PlaceBlood)
                Player.ReferenceHub.characterClassManager.RpcPlaceBlood(target.Position, 1, 2);
        }

        private void DrawAttack(Vector3 target)
        {
            var cameraTransform = Player.CameraTransform;
            var cameraPosition = cameraTransform.position;

            float distance = Vector3.Distance(cameraPosition, target);
            while (distance > 0)
            {
                points.Add((cameraTransform.forward * distance) + cameraPosition);
                distance -= config.AttackSettings.OrbSpacing;
            }

            for (int i = 0; i < points.Count; i++)
            {
                Player.ReferenceHub.weaponManager.RpcPlaceDecal(false, 1, points[i], Quaternion.FromToRotation(Vector3.up, Player.Rotation));
            }

            points.Clear();
        }

        private IEnumerator<float> UpdateBurn()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);

                foreach (Player player in Player.List)
                {
                    if (!IsTargetable(player))
                        continue;

                    if (!(BurningHandler.Get(player) is BurningHandler burningHandler))
                        continue;

                    if (Vector3.Distance(Player.Position, player.Position) > config.Scp457Settings.BurnRadius)
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
            while (CombustCooldown > 0)
            {
                yield return Timing.WaitForSeconds(1f);
                CombustCooldown--;
            }
        }
    }
}