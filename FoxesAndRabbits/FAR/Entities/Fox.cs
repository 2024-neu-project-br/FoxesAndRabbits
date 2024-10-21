using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Fox : Entity {
        
        public Fox(GameInstance instance, int[] initialPos) : base(instance, EntityType.FOX, initialPos) {

            typeString = "FOX";
            maxFoodLevel = 10;
            foodLevel = maxFoodLevel;
            foodLevelNew = foodLevel; // huh? (this is probably some initialization stuff, im not entirely sure why im adding this, skibidi)

        }

        public override void IndividualUpdate() {

            

        }

    }

}