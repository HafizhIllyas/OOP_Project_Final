using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class BasicEnemy : EnemyAbstract
    {
        public BasicEnemy(Vector2 startPos)
        {
            Position = startPos;
            Health = 30;   
            Damage = 5;    
            Speed = 80f;   
        }

        public override void Update(float deltaTime, Vector2 playerPos)
        {
            // Movement Logic Of Enemy
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