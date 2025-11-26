using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class DamageBoostItem : ItemAbstract
    {
        public float BoostAmount { get; set; } = 5f;

        public DamageBoostItem(Vector2 pos) : base(pos)
        {
            Color = Color.Red;
        }

        public override void ApplyEffect(Player player)
        {
            // Permanent damage upgrade
            player.Damage += BoostAmount;
            System.Diagnostics.Debug.WriteLine($"Damage Upgraded! Now: {player.Damage}");
        }
    }
}