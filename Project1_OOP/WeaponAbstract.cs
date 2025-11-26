using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;



namespace Project1_OOP
{
    public abstract class WeaponAbstract
    {
        // === STATS ===
        public float Damage { get; set; }
        public float FireRate { get; set; }
        public float Range { get; set; }

        // === AMMO & RELOAD (NEW) ===
        public int MaxAmmo { get; set; }
        public int CurrentAmmo { get; set; }
        public float ReloadTime { get; set; }
        public bool IsReloading { get; set; } = false;
        protected float reloadTimer = 0f;

        // Specific weapon stats
        public int PelletCount { get; set; } = 1;
        public float SpreadAngle { get; set; }
        public float BurnDamage { get; set; }
        public float BurnDuration { get; set; }
        public float ExplosionRadius { get; set; }
        public float ExplosionDamage { get; set; }

        protected float fireTimer;

       


        public virtual void Update(float deltaTime, Vector2 position, List<Projectile> projectiles)
        {
            UpdateTimer(deltaTime);
        }

        public void UpdateTimer(float deltaTime)
        {
            // 1. Handle Reloading
            if (IsReloading)
            {
                reloadTimer -= deltaTime;
                if (reloadTimer <= 0)
                {
                    IsReloading = false;
                    CurrentAmmo = MaxAmmo;
                    System.Diagnostics.Debug.WriteLine("Reload Complete!");
                }
                return; // Stop here if reloading
            }

            // 2. Handle Fire Rate Cooldown
            fireTimer -= deltaTime;
        }

        // Call this when the player presses 'R'
        public void Reload()
        {
            if (!IsReloading && CurrentAmmo < MaxAmmo)
            {
                IsReloading = true;
                reloadTimer = ReloadTime;
                System.Diagnostics.Debug.WriteLine("Reloading...");
            }
        }

        // Helper to check if we can shoot
        public bool CanFire()
        {
            return !IsReloading && fireTimer <= 0 && CurrentAmmo > 0;
        }

        public void ConsumeAmmo()
        {
            CurrentAmmo--;
            fireTimer = FireRate; // Reset cooldown
        }

        
        public virtual void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles) { }

        protected Vector2 RotateVector(Vector2 direction, float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            return new Vector2(direction.X * cos - direction.Y * sin, direction.X * sin + direction.Y * cos);
        }
    }
}