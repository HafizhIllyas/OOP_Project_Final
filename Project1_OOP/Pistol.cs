using System;
using System.Collections.Generic;
using Project1_OOP;
using Microsoft.Xna.Framework;

public class Pistol : WeaponAbstract
{
    public Pistol()
    {
        Damage = 10;
        FireRate = 0.5f;
        Range = 300f;
        fireTimer = 0f;
        MaxAmmo = 12;
        ReloadTime = 1.5f;
        CurrentAmmo = 12;
    }

    

    public override void Fire(Vector2 position, Vector2 direction, List<Projectile> projectiles)
    {
      
     
        if (fireTimer <= 0)
        {
            fireTimer = FireRate; 

            
            projectiles.Add(new Projectile(position, direction, 400f, Damage, Range));
        }
    }
}