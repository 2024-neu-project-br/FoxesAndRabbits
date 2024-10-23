using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Entities;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR {

    public class Entity {
        
        public GameInstance instance;
        public Map map;

        public EntityType TYPE;
        public string typeString = "none";
        private int x, y, newX, newY;

        public int X {

            set { newX = map.GetWrappedX(value); }
            get { return x; }

        }
        public int Y {

            set { newY = map.GetWrappedY(value); }
            get { return y; }

        }

        private int fln = 0;
        public int foodLevel = 0, maxFoodLevel = 0;
        public int foodLevelNew {

            set {

                if (foodLevel + value > maxFoodLevel) fln = maxFoodLevel;
                else fln = value;

            }
            get { return fln; }

        }

        public bool hasMated = false, hasDied = false;

        public Entity(GameInstance instance, EntityType type, int[] initialPos) {

            this.instance = instance;
            map = instance.map;

            TYPE = type;
            x = map.GetWrappedX(initialPos[0]);
            y = map.GetWrappedY(initialPos[1]);
            newX = x;
            newY = y;

        }

        public void GenericMate(Func<Entity, int[], Entity> l) {

            List<Entity> nearbyEntities = map.GetEntitiesAround(X, Y);
            if (nearbyEntities.Count != 1) return;
            
            Entity mate = nearbyEntities[0];
            if (mate.TYPE != TYPE) return;
            if (map.GetEntitiesAround(mate.X, mate.Y).Count != 1) return;
            if (mate.hasMated) return;

            mate.hasMated = true;

            int[] childPos = map.GetRandomEmptyCellAround(X, Y);
            map.futureOccupancyMap[childPos[0], childPos[1]] = true;
            // this should never run lmao but im keeping it here (this statement would never return true because GetRandomEmptyCellAround(X, Y) has to always return a new cell due to the several previous checks for emptiness around the two pairs)
            if (childPos[0] == X && childPos[1] == Y) hasDied = true;

            Entity child = l(mate, childPos);
            map.AddNewEntity(child);

        }

        public void Update() {

            // stuff before individual updates
            foodLevelNew--;

            IndividualUpdate();

            // stuff after individual updates
            if (foodLevelNew <= 0 || hasDied) map.RemoveEntity(this);

            // this check here is only a failproof check, each entity should check whether if their future position is valid or not for themselves
            if (map.futureOccupancyMap[newX, newY]) {

                newX = x;
                newY = y;

                return;

            }

            map.futureOccupancyMap[newX, newY] = true; // balls

        }

        public virtual void IndividualUpdate() {}

        public void Tick() {

            foodLevel = foodLevelNew;
            x = newX;
            y = newY;

            hasMated = false;
            hasDied = false; // huh?

        }

    }

}