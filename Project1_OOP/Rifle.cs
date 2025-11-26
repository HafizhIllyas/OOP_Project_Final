using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class Rifle: WeaponAbstract
    {
        public float Accuracy {  get; set; }
        public int BurstCount { get; set; }

        public Rifle()
        {
            Damage = 15f;
            FireRate = 0.30f;
            Range = 400f;
            Accuracy = 0.9f;
            BurstCount = 3;
            MaxAmmo = 30;
            ReloadTime = 2.5f;
            CurrentAmmo = 30;
        }

        public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
        {
            Console.WriteLine($"Rifle: Firing {BurstCount} -- round burst");
            Random rand = new Random();

     

            if (fireTimer <= 0)
            {
                fireTimer = FireRate;
                projectiles.Add(new Projectile(position, direction, 600f, Damage, Range));
            }

        }
        
    }
}
