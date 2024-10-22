using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Rabbit : Entity {

        double diseaseProbability;
        public bool isDiseased;

        public Rabbit(GameInstance instance, int[] initialPos, double diseaseProbability) : base(instance, EntityType.RABBIT, initialPos) {

            typeString = "RABBIT";
            maxFoodLevel = 5;
            foodLevel = maxFoodLevel;
            foodLevelNew = foodLevel; // huh? (this is probably some initialization stuff, im not entirely sure why im adding this, skibidi)

            this.diseaseProbability = diseaseProbability;
            isDiseased = instance.random.NextDouble() < this.diseaseProbability;

        }

        public override void IndividualUpdate() {

            List<int[]> grassCellsAround = [.. instance.map.GetEmptyCellsAround(X, Y).OrderBy(x => instance.map.grassMap[x[0], x[1]])]; // this is based

            /*
            
                BUG: two entities can simultaneously choose to go on the same cell at once, and since the action isnt being taken until the Tick() function runs,
                the future location of each entity remains unknown unless a special variable denoting the subsequent is not utilised
                (my lazyass is probably too lazy to do that, maybe i will do it if i feel like)
            
                HOWEVER: this can lead to other bugs where the information of a select entity is not properly overhauled by the instance
                and therefore either gets stuck in the previous state or gets corrupted

                (i will actually fix it cus my dumbass aint playing with this bih, sooo...)
                SOLUTION: the presence of a matrix denoting the occupancy of each cell may bring a proper solution to the problem,
                where updating the current position of each entity would easily take use of the matrix and check if any other entity has already claimed the spot
                this check should be implemented in the Update() function of the Entity.cs class
                if a select entitys future position would conflict with the already selected coordinates of anothers, the entity in question should keep
                its momentary position and shall try again in the next iteration

                ينشاللله يا الله اكبر 🙏🙏🙏


            */

        }

    }

}