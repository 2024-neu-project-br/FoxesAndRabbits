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
        public void Eat()
        {
            switch (instance.map.grassMap[X, Y]) {
                case 2:
                    if (foodLevel != maxFoodLevel)
                    {
                        foodLevelNew++;
                        instance.map.grassMapNew[X, Y] = 0;
                    }
                break;
                case 3:
                    if (foodLevel != maxFoodLevel - 1) {
                        foodLevelNew++;
                        instance.map.grassMapNew[X, Y] = 0;
                    }
                    if (foodLevel != maxFoodLevel) foodLevelNew++;
                break;
            }
        }


        public void Move()
        {
            int[] randomCell = instance.map.GetRandomEmptyCellAround(X, Y, 1);
            X = randomCell[0];
            Y = randomCell[1];
            // I know I'm probably not doing this right, but please just leave this be for the time being, because I actually feel like I'll just kms after making the website part work
        }

    }

}