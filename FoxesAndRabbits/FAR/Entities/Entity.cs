using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Entities;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR {

    public class Entity {
        
        public GameInstance instance;

        public EntityType TYPE;
        private int x, y;

        public int X {

            set { x = instance.map.GetWrappedX(value); }
            get { return x; }

        }
        public int Y {

            set { y = instance.map.GetWrappedY(value); }
            get { return y; }

        }

        public int foodLevel = 0, foodLevelNew = 0;
        public bool hasMated = false, hasDied = false;

        public Entity(GameInstance instance, EntityType type, int[] initialPos) {

            this.instance = instance;

            TYPE = type;
            X = initialPos[0];
            Y = initialPos[1];

        }

    }

}