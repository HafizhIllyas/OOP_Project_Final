using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_OOP
{
    public abstract class AbilityAbstract
    {
        public string Name { get; set; }
        public int CurrentLevel { get; protected set; } = 0;

        
        public int MaxLevel { get; } = 5; // Ability Level Cap

        public abstract void ApplyBuff(Player player);

        public bool LevelUp(Player player)
        {
            if (CurrentLevel < MaxLevel)
            {
                CurrentLevel++;
                ApplyBuff(player);
                Console.WriteLine($"Ability {Name} upgraded to Level {CurrentLevel}!");
                return true; 
            }
            return false; 
        }
    }
}
