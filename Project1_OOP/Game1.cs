using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project1_OOP
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // --- Managers & Core ---
        EntityManager _entityManager; 
        Player player;

        // --- Weapons ---
        WeaponAbstract pistol, shotgun, rifle, flamethrower, minigun, grenadeLauncher;

        // --- Abilities ---
        DamageAbility abilityDamage;
        HealthAbility abilityHealth;
        SpeedAbility abilitySpeed;

        // --- Assets ---
        Texture2D backgroundTexture;
        SpriteFont gameFont;

        // --- Game State ---
        float gameTimer = 0f;
        float spawnTimer = 0f;
        float currentSpawnRate = 2.0f;
        Random random;
        enum GameState { Playing, LevelUp, GameOver }
        GameState currentState = GameState.Playing;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // 1. Init Managers
            _entityManager = new EntityManager();
            random = new Random();

            // 2. Init Weapons
            pistol = new Pistol(); 
            shotgun = new Shotgun();
            rifle = new Rifle();
            flamethrower = new Flamethrower();
            minigun = new Minigun();
            grenadeLauncher = new GrenadeLauncher();

            // 3. Init Player & Abilities
            player = new Player();
            player.Weapon = pistol;
            abilityDamage = new DamageAbility();
            abilityHealth = new HealthAbility();
            abilitySpeed = new SpeedAbility();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);

            // Load UI Assets
            gameFont = Content.Load<SpriteFont>("GameFont");
            Texture2D bulletTex = Content.Load<Texture2D>("bullet");
            Texture2D fireTex = Content.Load<Texture2D>("flamethrower_0005");
            Texture2D healthTex = Content.Load<Texture2D>("healthDrop");
            Texture2D damageTex = Content.Load<Texture2D>("damageDrop");
            Texture2D xpTex = Content.Load<Texture2D>("expDrop");
            Texture2D dmgUpTex = Content.Load<Texture2D>("bulletIcon");
            Texture2D healUpTex = Content.Load<Texture2D>("healthIcon");
            Texture2D spdUpTex = Content.Load<Texture2D>("flyingshoes");
            Texture2D whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new Color[] { Color.White });

            // Load Textures
            Texture2D tBasic = Content.Load<Texture2D>("zombie_move_0002"); 
            Texture2D tTank = Content.Load<Texture2D>("muscle_torso_1h");
            Texture2D tSpeedy = Content.Load<Texture2D>("speedyZombie");
            Texture2D tBoomer = Content.Load<Texture2D>("fasto_move_0002");
            Texture2D tBoss = Content.Load<Texture2D>("boss");
            

            // Load BG 
            try { backgroundTexture = Content.Load<Texture2D>("Project"); }
            catch
            {
                backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
                backgroundTexture.SetData(new Color[] { Color.DarkOrange });
            }

            // Passing Assets to Managers
            UIManager.Init(whitePixel, bulletTex, gameFont, dmgUpTex, healUpTex, spdUpTex);
            _entityManager.InitTextures(bulletTex, whitePixel, fireTex, tBasic, tTank, tSpeedy, tBoomer, tBoss, healthTex, damageTex, xpTex);
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var k = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || k.IsKeyDown(Keys.Escape))
                Exit();

            // State 1: Playing
            if (currentState == GameState.Playing)
            {
                // 1. Handle Level Up Check
                if (player.ReadyToLevelUp)
                {
                    currentState = GameState.LevelUp;
                    return;
                }

                // 2. Player Logic
                Vector2 dir = Vector2.Zero;
                if (k.IsKeyDown(Keys.W)) dir.Y -= 1;
                if (k.IsKeyDown(Keys.S)) dir.Y += 1;
                if (k.IsKeyDown(Keys.A)) dir.X -= 1;
                if (k.IsKeyDown(Keys.D)) dir.X += 1;
                player.Move(dir, delta, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                MouseState mouse = Mouse.GetState();
                Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
                player.LookAt(mousePos);

                // 3. Weapon Switch & Fire
                if (k.IsKeyDown(Keys.D1)) player.Weapon = pistol;
                if (k.IsKeyDown(Keys.D2)) player.Weapon = shotgun;
                if (k.IsKeyDown(Keys.D3)) player.Weapon = rifle;
                if (k.IsKeyDown(Keys.D4)) player.Weapon = flamethrower;
                if (k.IsKeyDown(Keys.D5)) player.Weapon = minigun;
                if (k.IsKeyDown(Keys.D6)) player.Weapon = grenadeLauncher;
                if (k.IsKeyDown(Keys.R)) player.Weapon.Reload();

               
                player.Weapon.Update(delta, player.Position, _entityManager.Projectiles);

                Vector2 aimDir = mousePos - player.Position;
                if (aimDir != Vector2.Zero) aimDir.Normalize();

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (player.Weapon.CanFire())
                    {
                        player.Weapon.Fire(player.Position, aimDir, _entityManager.Projectiles);
                        player.Weapon.ConsumeAmmo();
                    }
                    else if (player.Weapon.CurrentAmmo <= 0)
                    {
                        player.Weapon.Reload();
                    }
                }

                // 4. Update Entity World (Collisions, Movement)
                _entityManager.Update(delta, player, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                // 5. Spawning Logic
                gameTimer += delta;
                spawnTimer += delta;
                if (gameTimer > 60) currentSpawnRate = 1.0f;
                if (spawnTimer >= currentSpawnRate)
                {
                   
                    _entityManager.SpawnEnemy(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                    if (random.NextDouble() < 0.2)
                        _entityManager.SpawnRandomItem(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                    spawnTimer = 0;
                }

                // 6. Check Death
                if (player.Health <= 0) currentState = GameState.GameOver;
            }
            // State 2: Level Up
            else if (currentState == GameState.LevelUp)
            {
                if (k.IsKeyDown(Keys.D1)) { if (abilityDamage.LevelUp(player)) { player.ConfirmLevelUp(); currentState = GameState.Playing; } }
                else if (k.IsKeyDown(Keys.D2)) { if (abilityHealth.LevelUp(player)) { player.ConfirmLevelUp(); currentState = GameState.Playing; } }
                else if (k.IsKeyDown(Keys.D3)) { if (abilitySpeed.LevelUp(player)) { player.ConfirmLevelUp(); currentState = GameState.Playing; } }
            }
            // State 3: Game Over
            else if (currentState == GameState.GameOver)
            {
                if (k.IsKeyDown(Keys.Enter)) RestartGame();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            // 1. Draw Background
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            // 2. Draw World Entities 
            _entityManager.Draw(_spriteBatch);

            // 3. Draw Player
            player.Draw(_spriteBatch);

            // 4. Draw UI Overlay 
            if (currentState != GameState.GameOver) UIManager.DrawHUD(_spriteBatch, player);

            if (currentState == GameState.LevelUp)
                UIManager.DrawLevelUp(_spriteBatch, abilityDamage, abilityHealth, abilitySpeed);

            else if (currentState == GameState.GameOver)
                UIManager.DrawGameOver(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void RestartGame()
        {
            // Reset Entities 
            _entityManager.ClearAll();

            //Reset abilities
            abilityDamage = new DamageAbility();
            abilityHealth = new HealthAbility();
            abilitySpeed = new SpeedAbility();

            //Reset weapons
            pistol = new Pistol();
            shotgun = new Shotgun();
            rifle = new Rifle();
            flamethrower = new Flamethrower();
            minigun = new Minigun();
            grenadeLauncher = new GrenadeLauncher();

            //Reset Player
            player = new Player();
            player.LoadContent(Content);
            player.Weapon = pistol;


            // Reset State
            currentState = GameState.Playing;
            spawnTimer = 0f;
            gameTimer = 0f;


        }
    }
}