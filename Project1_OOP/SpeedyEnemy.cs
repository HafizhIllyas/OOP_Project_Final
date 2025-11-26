using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class SpeedyEnemy : EnemyAbstract
    {
        private float dashTimer = 0f;

        public SpeedyEnemy(Vector2 startPos)
        {
            Position = startPos;
            Health = 20;
            Damage = 5;
            Speed = 160f;
        }

        public override void Update(float deltaTime, Vector2 playerPos)
        {
            dashTimer += deltaTime;
            float currentSpeed = Speed;

            if (dashTimer > 2.0f && dashTimer < 2.5f)
            {
                currentSpeed *= 2f; //Doubling speed - "Dashing"
            }
            if (dashTimer > 3.0f) dashTimer = 0f; //Reset the timer

            Vector2 direction = playerPos - Position;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                Position += direction * currentSpeed * deltaTime;
            }

            LookAt(playerPos);
        }
    }
}
