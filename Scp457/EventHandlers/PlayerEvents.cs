// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp457.EventHandlers
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Interactables.Interobjects.DoorUtils;
    using Scp457.API;
    using UnityEngine;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        /// <summary>
        /// Handles all subscriptions.
        /// </summary>
        public void SubscribeEvents()
        {
            PlayerHandlers.ChangingRole += OnChangingRole;
            PlayerHandlers.Died += OnDied;
            PlayerHandlers.Destroying += OnDestroying;
            PlayerHandlers.MedicalItemUsed += OnMedicalItemUsed;
            PlayerHandlers.Shot += OnShot;
            PlayerHandlers.Spawning += OnSpawning;
            PlayerHandlers.Verified += OnVerified;
        }

        /// <summary>
        /// Handles all unsubscribing.
        /// </summary>
        public void UnsubscribeEvents()
        {
            PlayerHandlers.ChangingRole -= OnChangingRole;
            PlayerHandlers.Died -= OnDied;
            PlayerHandlers.Destroying -= OnDestroying;
            PlayerHandlers.MedicalItemUsed -= OnMedicalItemUsed;
            PlayerHandlers.Shot -= OnShot;
            PlayerHandlers.Spawning -= OnSpawning;
            PlayerHandlers.Verified -= OnVerified;
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (BurningHandler.Get(ev.Player) is BurningHandler burningHandler)
                burningHandler.BurnTime = 0f;
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (BurningHandler.Get(ev.Player) is BurningHandler burningHandler)
                burningHandler.Destroy();

            if (Scp457.Get(ev.Player) is Scp457 scp457)
                scp457.Destroy();
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (BurningHandler.Get(ev.Target) is BurningHandler burningHandler)
                burningHandler.BurnTime = 0f;

            if (Scp457.Get(ev.Target) is Scp457 scp457)
                scp457.Destroy();
        }

        private void OnMedicalItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (Plugin.Instance.Config.BurnSettings.HealedBy.Contains(ev.Item) &&
                BurningHandler.Get(ev.Player) is BurningHandler burningHandler)
                burningHandler.BurnTime = 0f;
        }

        private void OnShot(ShotEventArgs ev)
        {
            if (Scp457.Get(ev.Target) != null && ev.HitboxTypeEnum == HitBoxType.HEAD)
                ev.Damage /= 4;
        }

        private void OnSpawning(SpawningEventArgs ev)
        {
            if (!(Scp457.Get(ev.Player) is Scp457 scp457))
                return;

            if (ev.RoleType != RoleType.Scp0492)
            {
                scp457.Destroy();
                return;
            }

            DoorVariant door = Map.GetDoorByName(Plugin.Instance.Config.Scp457Settings.SpawnDoor);
            if (door == null)
            {
                Log.Error("Could not find the spawn door for Scp457!");
                return;
            }

            if (PlayerMovementSync.FindSafePosition(door.transform.position, out Vector3 pos, true))
                ev.Position = pos;
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            BurningHandler.Dictionary.Add(ev.Player, new BurningHandler(ev.Player));
        }
    }
}