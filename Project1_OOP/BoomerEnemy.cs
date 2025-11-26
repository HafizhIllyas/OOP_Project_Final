using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public class BoomerEnemy : EnemyAbstract
    {
        public float ExplosionRadius { get; set; } = 100f;
        public float ExplosionDamage { get; set; } = 25f;

        public BoomerEnemy(Vector2 startPos)
        {
            Position = startPos;
            Health = 25;
            Damage = 0;
            Speed = 120f;
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
    }
}
