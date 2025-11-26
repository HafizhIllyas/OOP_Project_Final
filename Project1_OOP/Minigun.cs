using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Project1_OOP
{
    public class Minigun : WeaponAbstract
    {
        Random rand = new Random();

        public Minigun()
        {
            Damage = 4;
            FireRate = 0.04f; 
            Range = 500f;
            fireTimer = 0f;
            MaxAmmo = 100;
            CurrentAmmo = 100;
            ReloadTime = 3.0f;
        }

        public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
        {
            if (fireTimer <= 0)
            {
                fireTimer = FireRate;

                // Slight random jitter (accuracy error)
                float spread = (float)(rand.NextDouble() * 10 - 5);
                Vector2 finalDir = RotateVector(direction, spread);

                projectiles.Add(new Projectile
                {
                    Position = position,
                    Velocity = finalDir * 600f,
                    Damage = Damage,
                    LifeTime = Range / 600f
                });
            }
        }
    }
}