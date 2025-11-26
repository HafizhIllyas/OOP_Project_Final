using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public class DamageAbility : AbilityAbstract
    {
        public DamageAbility() { Name = "Damage Boost"; }

        public override void ApplyBuff(Player player)
        {
            // Increase player damage by 5 every level
            player.Damage += 5f;

            // Also buff the current weapon slightly
            if (player.Weapon != null) player.Weapon.Damage += 2f;
        }
    }
}
