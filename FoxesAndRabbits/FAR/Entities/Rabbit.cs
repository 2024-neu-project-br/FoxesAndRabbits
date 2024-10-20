using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Rabbit : Entity {

        public Rabbit(GameInstance instance, int[] initialPos) : base(instance, EntityType.RABBIT, initialPos) {

            typeString = "RABBIT";
            maxFoodLevel = 5;
            foodLevel = maxFoodLevel;
            foodLevelNew = foodLevel; // huh? (this is probably some initialization stuff, im not entirely sure why im adding this, skibidi)

        }

        public override void IndividualUpdate() {

            bool canUwU = foodLevel == maxFoodLevel || instance.map.GetEntitiesAround(X, Y).Count == 0; // huh? rabbits cant pounce according to the game rules, im not sure what you did here

            
        
        }

    }

}