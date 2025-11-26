using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class Projectile
    {
        
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Damage { get; set; }
        public float LifeTime { get; set; }
        public bool IsActive { get; set; } = true;

        // Special Stats
        public bool IsBurning { get; set; } = false;
        public float BurnDamage { get; set; }
        public float BurnDuration { get; set; }
        public bool IsExplosive { get; set; } = false;
        public float ExplosionRadius { get; set; }
        public float ExplosionDamage { get; set; }

       
        public Projectile() { }

        
        public Projectile(Vector2 pos, Vector2 dir, float speed, float dmg, float range)
        {
            Position = pos;
            if (dir != Vector2.Zero) dir.Normalize();
            Velocity = dir * speed;
            Damage = dmg;
            LifeTime = range / speed; // Calculate lifetime based on range
        }

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
            LifeTime -= deltaTime;
            if (LifeTime <= 0) IsActive = false;
        }
    }
}