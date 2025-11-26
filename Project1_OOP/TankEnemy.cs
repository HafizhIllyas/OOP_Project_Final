using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class TankEnemy : EnemyAbstract
    {
        public float Armor { get; set; } = 2.5f;

        public TankEnemy(Vector2 startPos)
        {
            Position = startPos;
            Health = 75;
            Damage = 15;
            Speed = 40f;
            Width = 40;
            Height = 40;
        }

        public override void Update(float deltaTime, Vector2 playerPos)
        {
            Vector2 direction = playerPos - Position;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                Position += direction * Speed * deltaTime;
            }

            LookAt(playerPos);
        }

        public override void TakeDamage(float amount)
        {
            float actualDamage = amount - Armor;
            if (actualDamage < 1) actualDamage = 1;

            base.TakeDamage(actualDamage);
            System.Diagnostics.Debug.WriteLine($"Tank Blocked damage! Took {actualDamage}");
        }

    }
}
