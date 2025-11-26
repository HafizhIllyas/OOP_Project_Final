using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Project1_OOP
{
    public class GrenadeLauncher : WeaponAbstract
    {
        public GrenadeLauncher()
        {
            Damage = 50;          
            FireRate = 1.2f;      
            Range = 400f;
            ExplosionRadius = 80f;
            ExplosionDamage = 30f;
            fireTimer = 0f;
            MaxAmmo = 6;
            CurrentAmmo = 6;
            ReloadTime = 3.0f;
        }

        public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
        {
            if (fireTimer <= 0)
            {
                fireTimer = FireRate;

                projectiles.Add(new Projectile
                {
                    Position = position,
                    Velocity = direction * 350f,
                    Damage = Damage,
                    LifeTime = Range / 350f,
                    IsExplosive = true,          // Marks it as a grenade
                    ExplosionRadius = ExplosionRadius,
                    ExplosionDamage = ExplosionDamage
                });
            }
        }
    }
}