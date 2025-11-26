using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public class SpeedAbility : AbilityAbstract
    {
        public SpeedAbility() { Name = "Speed Boost"; }

        public override void ApplyBuff(Player player)
        {
            // Increase speed by 10%
            player.Speed += 15f;
        }
    }
}
