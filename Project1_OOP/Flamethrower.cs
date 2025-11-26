using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Project1_OOP
{
    public class Flamethrower : WeaponAbstract
    {
        Random rand = new Random();

        public Flamethrower()
        {
            Damage = 2;          
            FireRate = 0.05f;   
            Range = 250f;
            BurnDamage = 5f;
            BurnDuration = 3f;
            fireTimer = 0f;
            MaxAmmo = 200;
            CurrentAmmo = 200;
            ReloadTime = 3.0f;

        }

        public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
        {
            if (fireTimer <= 0)
            {
                fireTimer = FireRate;

                
                for (int i = 0; i < 3; i++)
                {
                    // Random spread between -15 and +15 degrees
                    float angle = (float)(rand.NextDouble() * 30 - 15);
                    Vector2 spreadDir = RotateVector(direction, angle);

                    projectiles.Add(new Projectile
                    {
                        Position = position,
                        Velocity = spreadDir * 250f, 
                        Damage = Damage,
                        LifeTime = Range / 250f,
                        IsBurning = true,
                        BurnDamage = BurnDamage,
                        BurnDuration = BurnDuration
                    });
                }
            }
        }
    }
}