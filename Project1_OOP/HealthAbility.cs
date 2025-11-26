using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public class HealthAbility : AbilityAbstract
    {
        public HealthAbility() { Name = "Health Boost"; }

        public override void ApplyBuff(Player player)
        {
            
            player.MaxHealth += 20f;
            player.Health = player.MaxHealth;
        }
    }
}
