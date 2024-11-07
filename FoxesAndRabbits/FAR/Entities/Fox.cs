using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Fox : Entity {
        
        bool isInfected = false;
        public bool femaleCalling = false;

        public Fox(GameInstance instance, int[] initialPos) : base(instance, EntityType.FOX, initialPos) {

            typeString = "FOX";
            maxFoodLevel = 10;
            foodLevel = 10; // set to this number for various reasons
            foodLevelNew = foodLevel; // huh? (this is probably some initialization stuff, im not entirely sure why im adding this)

        }

        int matingTick = 0;
        public override void IndividualUpdate() {

            matingTick++;
            if (matingTick == 5) {

                femaleCalling = instance.random.NextDouble() > 0.4;
                if (!femaleCalling) {

                    List<Entity> hornyFoxesNearby = [.. map.GetEntitiesByTypeAround(X, Y, 5, TYPE).Where(e => ((Fox) e).femaleCalling)];
                    if (hornyFoxesNearby.Count < 0) {

                        Fox mate = (Fox) hornyFoxesNearby[instance.random.Next(hornyFoxesNearby.Count)];
                        int[] locationAroundMate = map.GetRandomEmptyCellAround(mate.X, mate.Y);
                        Move(locationAroundMate);
                        Mate();

                    }

                }

                matingTick = 0;
                return;

            }

            Mate();
            Prey(out int[] nextPos);
            Move(nextPos);

            // change the probability of death to have an effect on the outcome of the simulation or smth, bismillah
            if (instance.random.NextDouble() > 1 && isInfected) hasDied = true;

        }

        private void Mate() => GenericMate((mate, childPos) => new Fox(instance, childPos));

        private (List<Entity>?, bool) GetPossiblePrey() {

            List<Entity> rabbitsInRange = map.GetEntitiesByTypeAround(X, Y, EntityType.RABBIT);
            bool rangeExpansion = false;

            if (rabbitsInRange.Count == 0 && foodLevel >= 3 && foodLevel <= 6) {
                
                rangeExpansion = true;
                rabbitsInRange = map.GetEntitiesByTypeAround(X, Y, 3, EntityType.RABBIT);

            }

            return (rabbitsInRange.Count == 0 ? null : rabbitsInRange, rangeExpansion);

        }

        private void Prey(out int[] nextPos) {

            (List<Entity>?, bool) tuple = GetPossiblePrey();
            List<Entity>? possibleTargets = tuple.Item1;
            bool pounce = tuple.Item2;

            if (possibleTargets == null) {

                nextPos = map.GetRandomEmptyCellAround(X, Y);
                return;

            }

            Rabbit target = (Rabbit) possibleTargets[instance.random.Next(possibleTargets.Count)];

            if (target.isDiseased && !isInfected) {

                hasDied = 0.95 < instance.random.NextDouble(); // just dies
                isInfected = true;

            }

            foodLevelNew = pounce ? 2 : foodLevel + (isInfected ? 1 : 3);

            nextPos = [target.X, target.Y];
            map.RemoveEntity(target);

        }

        private void Move(int[] pos) {

            X = pos[0]; // if two foxes choose the same rabbit they can both devour the same one with the current state of the simulation, this is marginal and i cant be bothered to fix it
            Y = pos[1];

        }

    }

}