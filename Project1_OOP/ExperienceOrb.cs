using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class ExperienceOrb : ItemAbstract
    {
        public int ExpAmount { get; set; } = 20;

        public ExperienceOrb(Vector2 pos) : base(pos)
        {
            Color = Color.Cyan;
        }

        public override void ApplyEffect(Player player)
        {
            player.GainEXP(ExpAmount);
        }
    }
}