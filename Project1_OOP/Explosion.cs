using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1_OOP
{
    public class Explosion
    {
        public Vector2 Position;
        public float Timer;
        public float MaxTime = 0.5f; // How long the explosion will last
        public float MaxRadius;
        public bool IsActive = true;

        public Explosion(Vector2 pos, float radius)
        {
            Position = pos;
            MaxRadius = radius;
            Timer = 0f;
        }

        public void Update(float delta)
        {
            Timer += delta;
            if (Timer >= MaxTime)
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            
            float progress = Timer / MaxTime;
            float currentSize = MaxRadius * 2 * (progress + 0.2f); // Starts small, gets big

            // Fade out
            float opacity = 1.0f - progress;
            Color color = Color.Orange * opacity;

            // Draw centered square representing the explosion
            Rectangle rect = new Rectangle(
                (int)(Position.X - currentSize / 2),
                (int)(Position.Y - currentSize / 2),
                (int)currentSize,
                (int)currentSize
            );

            spriteBatch.Draw(texture, rect, color);
        }
    }
}