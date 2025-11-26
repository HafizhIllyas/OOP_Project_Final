using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1_OOP
{
    public static class UIManager
    {
        private static Texture2D _pixel;      // The white square for bars/backgrounds
        private static Texture2D _bulletIcon; // The bullet image
        private static SpriteFont _font;      // The font for text
        private static Texture2D _dmgUp, _healUp, _spdUp;
        

        
        public static void Init(Texture2D pixel, Texture2D bulletIcon, SpriteFont font, Texture2D dmgUp, Texture2D healUp, Texture2D spdUp)
        {
            _pixel = pixel;
            _bulletIcon = bulletIcon;
            _font = font;
            _dmgUp = dmgUp;
            _healUp = healUp;
            _spdUp = spdUp;
        }

        // 1. Draw HUD
        public static void DrawHUD(SpriteBatch sb, Player player)
        {
            // 1.1 Draw Ammo 
            Color c = (player.Weapon.IsReloading || player.Weapon.CurrentAmmo == 0) ? Color.Red : Color.White;

            if (player.Weapon.MaxAmmo > 30)
                DrawAmmoBar(sb, player, c);
            else
                DrawBulletIcons(sb, player, c);

            // 1.2 Draw Health Bar
            int barWidth = 60;
            int barHeight = 8;
            Vector2 barPos = new Vector2(player.Position.X - barWidth / 2, player.Position.Y - 40);

            // Background (Red) 
            sb.Draw(_pixel, new Rectangle((int)barPos.X, (int)barPos.Y, barWidth, barHeight), Color.Red);

            // Foreground (Green)
            float hpPercent = player.Health / player.MaxHealth;
            if (hpPercent < 0) hpPercent = 0;
            sb.Draw(_pixel, new Rectangle((int)barPos.X, (int)barPos.Y, (int)(barWidth * hpPercent), barHeight), Color.LimeGreen);

            // 1.3 Draw XP Bar (Bottom of screen)
            int screenW = 1280; 
            int xpH = 20;
            int xpY = 720 - xpH;

            sb.Draw(_pixel, new Rectangle(0, xpY, screenW, xpH), Color.DarkGray); // BG

            if (player.expToNextLevel > 0)
            {
                float xpPercent = (float)player.Experience / (float)player.expToNextLevel;
                if (xpPercent > 1f) xpPercent = 1f;
                sb.Draw(_pixel, new Rectangle(0, xpY, (int)(screenW * xpPercent), xpH), Color.Cyan);
            }

            // 1.4 Draw Damage Stats 
            int blocks = (int)(player.Damage / 5);
            for (int i = 0; i < blocks; i++)
            {
                sb.Draw(_pixel, new Rectangle(10 + (i * 15), xpY - 20, 10, 10), Color.Red);
            }
        }

        // 2. Draw Level Up Screen
        public static void DrawLevelUp(SpriteBatch sb, AbilityAbstract dmg, AbilityAbstract hp, AbilityAbstract spd)
        {
            // Dim Background
            sb.Draw(_pixel, new Rectangle(0, 0, 1280, 720), Color.Black * 0.5f);

            int centerY = 720 / 2 - 50;
            int centerX = 1280 / 2;
            Vector2 txtOffset = new Vector2(10, 110);

            // 2.1 Damage (Red)
            sb.Draw(_dmgUp, new Rectangle(centerX - 200, centerY, 100, 100), Color.White);
            string t1 = $"1. Damage\nLvl {dmg.CurrentLevel}/{dmg.MaxLevel}";
            if (dmg.CurrentLevel >= dmg.MaxLevel) t1 = "MAXED";
            sb.DrawString(_font, t1, new Vector2(centerX - 200, centerY) + txtOffset, Color.White);

            // 2.2 Health (Green)
            sb.Draw(_healUp, new Rectangle(centerX - 50, centerY, 100, 100), Color.White);
            string t2 = $"2. Health\nLvl {hp.CurrentLevel}/{hp.MaxLevel}";
            if (hp.CurrentLevel >= hp.MaxLevel) t2 = "MAXED";
            sb.DrawString(_font, t2, new Vector2(centerX - 50, centerY) + txtOffset, Color.White);

            // 2.3 Speed (Blue)
            sb.Draw(_spdUp, new Rectangle(centerX + 100, centerY, 100, 100), Color.White);
            string t3 = $"3. Speed\nLvl {spd.CurrentLevel}/{spd.MaxLevel}";
            if (spd.CurrentLevel >= spd.MaxLevel) t3 = "MAXED";
            sb.DrawString(_font, t3, new Vector2(centerX + 100, centerY) + txtOffset, Color.White);
        }

        // === 3. DRAW GAME OVER SCREEN (Previously in Game1) ===
        public static void DrawGameOver(SpriteBatch sb)
        {
            sb.Draw(_pixel, new Rectangle(0, 0, 1280, 720), Color.Black * 0.7f);

            string msg = "GAME OVER";
            string sub = "Press ENTER to Restart";
            Vector2 center = new Vector2(1280 / 2, 720 / 2);

            // Center text logic
            Vector2 sz1 = _font.MeasureString(msg);
            Vector2 sz2 = _font.MeasureString(sub);

            sb.DrawString(_font, msg, center - sz1 / 2, Color.Red);
            sb.DrawString(_font, sub, center - sz2 / 2 + new Vector2(0, 40), Color.White);
        }

        // --- Helpers for Ammo ---
        private static void DrawBulletIcons(SpriteBatch sb, Player p, Color c)
        {
            for (int i = 0; i < p.Weapon.CurrentAmmo; i++)
            {
                // Note: Using _bulletIcon here specifically
                sb.Draw(_bulletIcon, new Vector2(20, 600 - (i * 20)), null, c * 0.8f, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
        }

        private static void DrawAmmoBar(SpriteBatch sb, Player p, Color c)
        {
            int h = 200; int w = 20; int x = 20; int y = 600 - h;
           
            sb.Draw(_pixel, new Rectangle(x, y, w, h), Color.Gray * 0.5f);

            float pct = (float)p.Weapon.CurrentAmmo / p.Weapon.MaxAmmo;
            int curH = (int)(h * pct);

            sb.Draw(_pixel, new Rectangle(x, y + (h - curH), w, curH), c);
        }
    }
}