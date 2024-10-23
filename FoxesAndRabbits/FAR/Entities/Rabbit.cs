using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Rabbit : Entity {

        public bool isDiseased;

        public Rabbit(GameInstance instance, int[] initialPos, double diseaseProbability) : base(instance, EntityType.RABBIT, initialPos) {

            typeString = "RABBIT";
            maxFoodLevel = 5;
            foodLevel = 1;
            foodLevelNew = foodLevel; // huh? (this is probably some initialization stuff, im not entirely sure why im adding this, skibidi)

            isDiseased = instance.random.NextDouble() < diseaseProbability;

        }

        public override void IndividualUpdate() {

            Mate();
            Move(out int[] choice);
            Eat(choice);

            if (isDiseased && instance.random.NextDouble() > 0.6) hasDied = true; // *randomly dies*

        }

        private void Mate() => GenericMate((mate, childPos) => {

            double diseaseProbability = (isDiseased ? 0.4 : 0.0) + (((Rabbit) mate).isDiseased ? 0.4 : 0.0) + 0.1;

            //double diseaseProbability = this.diseaseProbability + ((Rabbit) mate).diseaseProbability;

            return new Rabbit(instance, childPos, diseaseProbability);

        });

        private void Move(out int[] choice) {

            List<int[]> emptyCellsInRange = map.GetEmptyCellsAround(X, Y);
            if (emptyCellsInRange.Count == 0) {

                hasDied = true; // dies cus of overcramming
                choice = [X, Y];
                return;

            }

            int[] maxGrassCell = emptyCellsInRange.MaxBy(cell => map.grassMap[cell[0], cell[1]])!;
            int maxGrassCellValue = map.grassMap[maxGrassCell[0], maxGrassCell[1]];

            List<int[]> bestGrassCellsInRange = [.. emptyCellsInRange.Where(cell => map.grassMap[cell[0], cell[1]] == maxGrassCellValue)];
            choice = bestGrassCellsInRange[instance.random.Next(bestGrassCellsInRange.Count)];

            X = choice[0];
            Y = choice[1]; // this here could bug out, modify after testing Zraphy

        }

        private void Eat(int[] choice) {

            if (choice[0] == X && choice[1] == Y) return;

            map.grassMapNew[choice[0], choice[1]] = 1;
            int foodLevelAtChoice = map.grassMap[choice[0], choice[1]];

            if (isDiseased) {

                foodLevelNew = foodLevelAtChoice <= 0 ? 0 : 1;
                return;

            }

            foodLevelNew = foodLevel + foodLevelAtChoice - 1;

        }

    }

}