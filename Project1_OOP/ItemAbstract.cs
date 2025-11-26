using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1_OOP
{
    public abstract class ItemAbstract
    {
        public Vector2 Position { get; set; }
        public bool IsActive { get; set; } = true;
        public Color Color { get; set; } = Color.White; // Color determines the item type visually

        public ItemAbstract(Vector2 pos)
        {
            Position = pos;
        }

        public abstract void ApplyEffect(Player player);
    }
}