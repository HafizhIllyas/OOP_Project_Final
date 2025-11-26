using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Project1_OOP
{
    public class EntityManager
    {
        public List<EnemyAbstract> Enemies { get; private set; }
        public List<Projectile> Projectiles { get; private set; }
        public List<ItemAbstract> Items { get; private set; }
        public List<Explosion> Explosions { get; private set; }

        private Random _random;
        private Texture2D _projTex, _pixelTex, _flameTex;
        private static Texture2D _texBasic;
        private static Texture2D _texTank;
        private static Texture2D _texSpeedy;
        private static Texture2D _texBoomer;
        private static Texture2D _texBoss;
        private static Texture2D _texHealth;
        private static Texture2D _texDamage;
        private static Texture2D _texExp;

        public EntityManager()
        {
            Enemies = new List<EnemyAbstract>();
            Projectiles = new List<Projectile>();
            Items = new List<ItemAbstract>();
            Explosions = new List<Explosion>();
            _random = new Random();
        }

        public void InitTextures(Texture2D projTex, Texture2D pixel, Texture2D flame, Texture2D basic, Texture2D tank, Texture2D speedy, Texture2D boomer, Texture2D boss, Texture2D health, Texture2D damage, Texture2D exp)
        {
            
            _projTex = projTex;
            _pixelTex = pixel;
            _flameTex = flame;

            _texBasic = basic;
            _texTank = tank;
            _texSpeedy = speedy;
            _texBoomer = boomer;
            _texBoss = boss;

            _texHealth = health;
            _texDamage = damage;
            _texExp = exp;
        }

        public void ClearAll()
        {
            Enemies.Clear();
            Projectiles.Clear();
            Items.Clear();
            Explosions.Clear();
        }


        public void Update(float delta, Player player, int screenW, int screenH)
        {
            UpdateEnemies(delta, player);
            UpdateProjectiles(delta);
            UpdateItems(player);
            UpdateExplosions(delta);
            CheckCollisions(player);
        }
        public void Draw(SpriteBatch sb)
        {
            // 1. Projectiles
            foreach (var p in Projectiles)
            {
                Texture2D currentTexProj = _projTex;
                float scale = 1.0f;
                Color c = Color.White;

                if (p.IsBurning)
                {
                    currentTexProj = _flameTex;
                    scale = 0.1f;

                }
                float rot = (float)Math.Atan2(p.Velocity.Y, p.Velocity.X);
                Vector2 origin = new Vector2(currentTexProj.Width / 2, currentTexProj.Height / 2);
                if (p.IsExplosive) c = Color.Orange;
                sb.Draw(currentTexProj, p.Position, null, c, rot, origin, scale, SpriteEffects.None, 0f);
            }

            // 2. Explosions
            foreach (var exp in Explosions) exp.Draw(sb, _pixelTex);

            // 3. Enemies
            foreach (var e in Enemies)
            {
                Texture2D currentTexEnemy = _texBasic;

                // Switch texture based on type
                if (e is TankEnemy) currentTexEnemy = _texTank;
                else if (e is SpeedyEnemy) currentTexEnemy = _texSpeedy;
                else if (e is BoomerEnemy) currentTexEnemy = _texBoomer;
                else if (e is BossEnemy) currentTexEnemy = _texBoss;

                Vector2 origin = new Vector2(currentTexEnemy.Width / 2, currentTexEnemy.Height / 2);
                //sb.Draw(currentTexEnemy, new Rectangle((int)e.Position.X, (int)e.Position.Y, e.Width, e.Height), Color.White);
                sb.Draw(
                        currentTexEnemy,
                        e.Position,
                        null,
                        Color.White,
                        e.Rotation, 
                        origin,     
                        1.0f,       
                        SpriteEffects.None,
                        0f
    );
                }

            // 4. Items
            foreach (var item in Items)
            {
                Texture2D currentTex = _pixelTex; 
                Color tint = Color.White;         

                if (item is HealthKit)
                {
                    currentTex = _texHealth;
                }
                else if (item is DamageBoostItem)
                {
                    currentTex = _texDamage;
                }
                else if(item is ExperienceOrb)
                {
                    currentTex = _texExp;
                  
                }
                sb.Draw(currentTex, new Rectangle((int)item.Position.X, (int)item.Position.Y, 15, 15), tint);
            }
        }

        // Logic Methods

        public void SpawnEnemy(int screenWidth, int screenHeight)
        {
            int edge = _random.Next(0, 4);
            Vector2 spawnPos = Vector2.Zero;
            int offset = 50;

            switch (edge)
            {
                case 0: spawnPos = new Vector2(_random.Next(0, screenWidth), -offset); break;
                case 1: spawnPos = new Vector2(_random.Next(0, screenWidth), screenHeight + offset); break;
                case 2: spawnPos = new Vector2(-offset, _random.Next(0, screenHeight)); break;
                case 3: spawnPos = new Vector2(screenWidth + offset, _random.Next(0, screenHeight)); break;
            }


            int roll = _random.Next(0, 100);
            if (roll < 60) Enemies.Add(new BasicEnemy(spawnPos));
            else if (roll < 75) Enemies.Add(new SpeedyEnemy(spawnPos));
            else if (roll < 90) Enemies.Add(new TankEnemy(spawnPos));
            else if (roll < 99) Enemies.Add(new BoomerEnemy(spawnPos));
            else Enemies.Add(new BossEnemy(spawnPos));
        }

        public void SpawnRandomItem(int screenWidth, int screenHeight)
        {
            int x = _random.Next(50, screenWidth - 50);
            int y = _random.Next(50, screenHeight - 50);
            Vector2 pos = new Vector2(x, y);

            if (_random.NextDouble() > 0.5) Items.Add(new HealthKit(pos));
            else Items.Add(new DamageBoostItem(pos));
        }

        private void UpdateEnemies(float delta, Player player)
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemies[i].Update(delta, player.Position);
                if (!Enemies[i].IsActive)
                {
                    if (Enemies[i] is BoomerEnemy boomer)
                    {
                        CreateExplosion(boomer.Position, boomer.ExplosionRadius, boomer.ExplosionDamage, player);
                        float dist = Vector2.Distance(player.Position, boomer.Position);
                        if (dist < boomer.ExplosionRadius) player.TakeDamage(boomer.ExplosionDamage);
                    }
                    
                    Items.Add(new ExperienceOrb(Enemies[i].Position));
                    Enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateProjectiles(float delta)
        {
            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                Projectiles[i].Update(delta);
                if (!Projectiles[i].IsActive) Projectiles.RemoveAt(i);
            }
        }

        private void UpdateExplosions(float delta)
        {
            for (int i = Explosions.Count - 1; i >= 0; i--)
            {
                Explosions[i].Update(delta);
                if (!Explosions[i].IsActive) Explosions.RemoveAt(i);
            }
        }

        private void UpdateItems(Player player)
        {
            Rectangle playerRect = new Rectangle((int)player.Position.X - 16, (int)player.Position.Y - 16, 32, 32);
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                Rectangle itemRect = new Rectangle((int)Items[i].Position.X, (int)Items[i].Position.Y, 15, 15);
                if (playerRect.Intersects(itemRect))
                {
                    Items[i].ApplyEffect(player);
                    Items.RemoveAt(i);
                }
            }
        }

        private void CheckCollisions(Player player)
        {
            if (player == null) return;
            Rectangle playerRect = new Rectangle((int)player.Position.X - 16, (int)player.Position.Y - 16, 32, 32);

            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                EnemyAbstract e = Enemies[i];
                Rectangle enemyRect = new Rectangle((int)e.Position.X, (int)e.Position.Y, e.Width, e.Height);

                // Player vs Enemy
                if (playerRect.Intersects(enemyRect))
                {
                    player.TakeDamage(e.Damage * 0.05f);
                }

                // Bullet vs Enemy
                for (int j = Projectiles.Count - 1; j >= 0; j--)
                {
                    Projectile p = Projectiles[j];
                    Rectangle projRect = new Rectangle((int)p.Position.X, (int)p.Position.Y, 10, 10);

                    if (projRect.Intersects(enemyRect))
                    {
                        e.TakeDamage(p.Damage);
                        p.IsActive = false;

                        if (p.IsExplosive)
                            CreateExplosion(p.Position, p.ExplosionRadius, p.ExplosionDamage, player);

                        if (e.Health <= 0) break;
                    }
                }
            }
        }

        private void CreateExplosion(Vector2 center, float radius, float damage, Player player)
        {
            Explosions.Add(new Explosion(center, radius));
            foreach (var enemy in Enemies)
            {
                float distance = Vector2.Distance(center, enemy.Position);
                if (distance <= radius) enemy.TakeDamage(damage);
            }
        }
    }
}

