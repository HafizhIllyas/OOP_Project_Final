using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1_OOP
{
    public class Player
    {
        
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public Vector2 Position { get; set; }

        public WeaponAbstract Weapon { get; set; }
        public WeaponAbstract SignatureWeapon { get; set; }

        public int expToNextLevel;

       
        public Texture2D Texture { get; private set; }
        public Vector2 Origin { get; private set; }

        
        public float Rotation { get; set; }

        public Player()
        {
            Health = 100f;
            MaxHealth = 100f;
            Damage = 15f;
            Speed = 100f;
            Experience = 0;
            Level = 1;
            expToNextLevel = 100;

            Position = new Vector2(300, 300);
        }

        //Loading Sprite 
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = content.Load<Texture2D>("player");
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        // Drawing Player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null)
                return;

            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.White,
                Rotation,
                Origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        // Movement
        public void Move(Vector2 direction, float deltaTime, int mapWidth, int mapHeight)
        {
            // 1. Apply Movement
            if (direction.Length() > 0)
            {
                direction.Normalize();
                Position += direction * Speed * deltaTime;
            }

            // 2. Clamp Position (Keep player on screen)
            // Calculate the half-size so the sprite stops at the edge, not the center
            float halfW = 0;
            float halfH = 0;

            // Only calculate if texture is loaded to prevent crash
            if (Texture != null)
            {
                halfW = Texture.Width / 2f;
                halfH = Texture.Height / 2f;
            }

            // Clamp X and Y so the player cannot leave the rectangle
            Position = new Vector2(
                MathHelper.Clamp(Position.X, halfW, mapWidth - halfW),
                MathHelper.Clamp(Position.Y, halfH, mapHeight - halfH)
            );
        }

        // Combat & Leveling
        public void Attack()
        {
            if (Weapon != null)
            {
                Console.WriteLine($"Player attacks with {Weapon.GetType().Name} for {Damage + Weapon.Damage} total damage!");
            }
        }

        public bool ReadyToLevelUp { get; set; } = false;
        public void GainEXP(int amount)
        {
            Experience += amount;
            Console.WriteLine($"Gained {amount} EXP. Total: {Experience}/{expToNextLevel}");

            if(Experience >= expToNextLevel)
            {
                ReadyToLevelUp = true;
            }
        }

        public void ConfirmLevelUp()
        {
            Level++;
            Experience -= expToNextLevel;
            expToNextLevel = (int)(expToNextLevel * 1.5f);
            ReadyToLevelUp = false;
        }

        private void LevelUp()
        {
            Level++;
            Experience -= expToNextLevel;
            expToNextLevel = (int)(expToNextLevel * 1.5f);

            MaxHealth += 10f;
            Health = MaxHealth;
            Damage += 5f;
            Speed += 0.5f;

            Console.WriteLine($"LEVEL UP! Now level {Level}");
            Console.WriteLine($"Stats - HP: {MaxHealth}, DMG: {Damage}, SPD: {Speed}");
        }

        public void TakeDamage(float dmg)
        {
            Health -= dmg;
            Console.WriteLine($"Player took {dmg} damage! Health: {Health}/{MaxHealth}");

            if (Health <= 0)
            {
                Console.WriteLine("GAME OVER!");
            }
        }

        public void SwitchWeapon(WeaponAbstract newWeapon){
            Weapon = newWeapon;
            Console.WriteLine($"Switched to {newWeapon.GetType().Name}");
        }

        public void UseSignatureWeapon()
        {
            if (SignatureWeapon != null)
            {
                WeaponAbstract temp = Weapon;
                Weapon = SignatureWeapon;
                Attack();
                Weapon = temp;
            }
        }

        public void LookAt(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - Position;

            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}
