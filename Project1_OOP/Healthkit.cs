using Microsoft.Xna.Framework;

namespace Project1_OOP
{
    public class HealthKit : ItemAbstract
    {
        public float HealAmount { get; set; } = 40f;

        public HealthKit(Vector2 pos) : base(pos)
        {
            Color = Color.LimeGreen;
        }

        public override void ApplyEffect(Player player)
        {
            player.Health += HealAmount;

            // Don't go over max health
            if (player.Health > player.MaxHealth)
                player.Health = player.MaxHealth;

            System.Diagnostics.Debug.WriteLine("Picked up HealthKit!");
        }
    }
}