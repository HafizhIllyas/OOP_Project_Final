using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Project1_OOP
{
    public class Shotgun : WeaponAbstract
    {
       

        public Shotgun()
        {
            Damage = 8;
            FireRate = 1.5f;   
            Range = 250f;
            PelletCount = 5;
            SpreadAngle = 30f;
            MaxAmmo = 6;
            ReloadTime = 1.5f;
            CurrentAmmo = 6;

            
            fireTimer = 0f;
        }

        

        public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
        {
            
            if (fireTimer <= 0)
            {
                
                fireTimer = FireRate;

                // Shoot logic so it spreads at an angle
                float angleStep = SpreadAngle / (PelletCount - 1);
                float startAngle = -SpreadAngle / 2f;

                for (int i = 0; i < PelletCount; i++)
                {
                    float angle = startAngle + (angleStep * i);
                    Vector2 spreadDir = RotateVector(direction, angle);
                    projectiles.Add(new Projectile(position, spreadDir, 350f, Damage, Range));
                }
            }
        }
    }
}