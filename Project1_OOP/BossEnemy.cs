using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public class BossEnemy : EnemyAbstract
    {
        public BossEnemy(Vector2 startPos)
        {
            Position = startPos;
            Health = 500;
            Damage = 25;
            Speed = 30f;
            Width = 70;
            Height = 70;
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
