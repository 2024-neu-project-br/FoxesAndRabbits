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

        public EntityType TYPE;
        public string typeString = "none";
        private int x, y, newX, newY;

        public int X {

            set { newX = instance.map.GetWrappedX(value); }
            get { return x; }

        }
        public int Y {

            set { newY = instance.map.GetWrappedY(value); }
            get { return y; }

        }

        public int foodLevel = 0, foodLevelNew = 0, maxFoodLevel = 0;
        public bool hasMated = false;

        public Entity(GameInstance instance, EntityType type, int[] initialPos) {

            this.instance = instance;

            TYPE = type;
            x = instance.map.GetWrappedX(initialPos[0]);
            y = instance.map.GetWrappedY(initialPos[1]);
            newX = x;
            newY = y;

        }

        public void Update() {

            // stuff before individual updates
            foodLevelNew--;

            IndividualUpdate();

            // stuff after individual updates
            if (foodLevelNew <= 0) instance.map.entitiesToBeRemoved.Add(this);

        }

        public virtual void IndividualUpdate() {}

        public void Tick() {

            foodLevel = foodLevelNew;
            x = newX;
            y = newY;

            hasMated = false;

        }

    }

}