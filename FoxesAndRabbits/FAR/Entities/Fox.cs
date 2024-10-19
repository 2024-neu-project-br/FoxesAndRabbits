using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Fox : Entity {
        
        public Fox(GameInstance instance, int[] initialPos) : base(instance, EntityType.FOX, initialPos) {

            maxFoodLevel = 10;
            foodLevel = maxFoodLevel;

        }

        public override void IndividualUpdate() {

            bool canPounce = foodLevel == 10 || instance.map.GetEntitiesAround(X, Y).Count == 0;

            // your stuff goes here zraphy

        }

    }

}