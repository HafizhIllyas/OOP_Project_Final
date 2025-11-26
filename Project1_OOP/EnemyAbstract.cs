using System;
using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public abstract class EnemyAbstract
    {
       // Shared Stats 
        public Vector2 Position { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public bool IsActive { get; set; } = true;

        // Dimensions for collision
        public int Width { get; set; } = 30;
        public int Height { get; set; } = 30;

        public float Rotation { get; set; }


        public abstract void Update(float deltaTime, Vector2 playerPos);

        
        public virtual void TakeDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0) IsActive = false;
        }

        public void LookAt(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - Position;

            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}