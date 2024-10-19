using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display;

namespace FoxesAndRabbits.FAR.Game {
    
    public class GameInstance {

        public string name;
        private int width, height;

        public Map map;

        public GameInstance(string name, int width, int height, bool isMapBlank) {

            this.name = name;
            this.width = width;
            this.height = height;

            map = new Map(this, width, height);
            if (!isMapBlank) InitRandom();

        }

        public void InitRandom() {

            

        }

        public void Tick() {}

        public void Update() {}

        public byte[] State() {

            return new byte[2];

        }

    }

}