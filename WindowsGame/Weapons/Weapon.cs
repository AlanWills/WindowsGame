﻿using _2DEngine;
using GameData;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    public enum WeaponState
    {
        kReady,
        kFiring,
        kReloading
    }

    /// <summary>
    /// A class that is used to store data and update logic for a weapon.
    /// All drawing will be done by the character so this does not need to be added to the screen manager, but rather be added onto a character class.
    /// It handles all the bullets that the weapon fires.
    /// </summary>
    public class Weapon : ObjectManager<Bullet>
    {
        #region Properties and Fields

        /// <summary>
        /// The data for this weapon.
        /// </summary>
        public WeaponData WeaponData { get; private set; }

        /// <summary>
        /// The data for the bullet this weapon fires - don't want to load this for every bullet we fire.
        /// </summary>
        private BulletData BulletData { get; set; }

        /// <summary>
        /// The data asset for this weapon.
        /// </summary>
        private string DataAsset { get; set; }

        /// <summary>
        /// The current amount of time that has passed since the last shot.
        /// </summary>
        private float CurrentFireTimer { get; set; }

        /// <summary>
        /// The current amount of time that has passed since reloading.
        /// </summary>
        private float CurrentReloadTimer { get; set; }

        /// <summary>
        /// The current number of bullets left in our magazine.
        /// </summary>
        public int CurrentBulletsInMagazine { get; private set; }

        /// <summary>
        /// The current state of the weapon.
        /// </summary>
        public WeaponState CurrentWeaponState { get; private set; }

        /// <summary>
        /// The previous state of the weapon.
        /// </summary>
        public WeaponState PreviousWeaponState { get; private set; }

        /// <summary>
        /// A reference to the armed character using this weapon.
        /// </summary>
        private ArmedCharacter ArmedCharacter { get; set; }

        #endregion

        public Weapon(ArmedCharacter armedCharacter, string dataAsset) :
            base()
        {
            CurrentWeaponState = WeaponState.kReady;
            PreviousWeaponState = CurrentWeaponState;
            DataAsset = dataAsset;
            ArmedCharacter = armedCharacter;
        }

        #region Virtual Functions

        /// <summary>
        /// Load the weapon data and set up our properties
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            DebugUtils.AssertNotNull(DataAsset);
            WeaponData = AssetManager.GetData<WeaponData>(DataAsset);
            DebugUtils.AssertNotNull(WeaponData);

            CurrentBulletsInMagazine = WeaponData.MagazineSize;

            BulletData = AssetManager.GetData<BulletData>(WeaponData.BulletDataAsset);
            DebugUtils.AssertNotNull(BulletData);
            DebugUtils.AssertNotNull(BulletData.TextureAsset);

            base.LoadContent();
        }

        /// <summary>
        /// Handle firing and reloading
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // Very important we do this here, otherwise we will not get representative information about the previous and current state (if we do it in Update for example)
            PreviousWeaponState = CurrentWeaponState;

            // If we are firing or reloading, then we should not be able to fire or reload until we are ready, so return
            if (CurrentWeaponState != WeaponState.kReady)
            {
                return;
            }

            // Should only be able to fire or reload, not both
            if (GameMouse.Instance.IsClicked(InputMap.Fire))
            {
                if (CurrentBulletsInMagazine > 0)
                { 
                    // If we have enough bullets, then fire one
                    Fire();
                }
            }
            else if (GameKeyboard.IsKeyPressed(InputMap.Reload))
            {
                Reload();
            }
        }

        /// <summary>
        /// Updates our gun's state and our bullets.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            switch (CurrentWeaponState)
            {
                // If we are ready then reload if we have no bullets left
                case WeaponState.kReady:
                    if (CurrentBulletsInMagazine == 0)
                    {
                        Reload();
                    }
                    break;

                // If we are firing, update the CurrentFireTimer and become ready to fire if we can
                case WeaponState.kFiring:
                {
                    CurrentFireTimer += elapsedGameTime;
                    if (CurrentFireTimer >= WeaponData.TimeBetweenShots)
                    {
                        FiringComplete();
                    }
                    break;
                }
                
                // If we are reloading, update the CurrentReloadTimer and become ready to fire if we can
                case WeaponState.kReloading:
                {
                    CurrentReloadTimer += elapsedGameTime;
                    if (CurrentReloadTimer >= WeaponData.ReloadTime)
                    {
                        ReloadComplete();
                    }
                    break;
                }
            }
        }

        #endregion

        #region Weapon Fire and Reload functions

        /// <summary>
        /// Fires a bullet and updates the weapon state and fire timer
        /// </summary>
        private void Fire()
        {
            DebugUtils.AssertNotNull(ArmedCharacter.PhysicsBody);

            CurrentWeaponState = WeaponState.kFiring;
            CurrentFireTimer = 0;
            CurrentBulletsInMagazine--;

            Bullet bullet = AddObject(new Bullet(ArmedCharacter.WorldPosition, BulletData.TextureAsset), true, true);
            DebugUtils.AssertNotNull(bullet.PhysicsBody);

            bullet.PhysicsBody.LinearVelocity = new Vector2(BulletData.Speed * ArmedCharacter.PhysicsBody.Direction, 0);
        }

        /// <summary>
        /// A function which encapsulates all the behaviour that should happen when we have finished firing
        /// </summary>
        private void FiringComplete()
        {
            CurrentWeaponState = WeaponState.kReady;
        }

        /// <summary>
        /// Reloads the weapon and updates the weapon state and reload timer
        /// </summary>
        private void Reload()
        {
            CurrentWeaponState = WeaponState.kReloading;
            CurrentReloadTimer = 0;
        }

        /// <summary>
        /// A function which encapsulates all the behaviour that should happen when we have finished reloading
        /// </summary>
        private void ReloadComplete()
        {
            CurrentWeaponState = WeaponState.kReady;
            CurrentFireTimer = WeaponData.TimeBetweenShots;
            CurrentBulletsInMagazine = WeaponData.MagazineSize;
        }

        #endregion
    }
}