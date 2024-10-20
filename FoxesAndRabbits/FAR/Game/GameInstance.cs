using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Update() => map.Update();

        public void Tick() => map.Tick();

        public byte[] State() {

            string state = "GRASSMAP\n";
            for (int y = 0; y < height; y++) {

                string[] row = new string[width];
                for (int x = 0; x < width; x++) row[x] = map.grassMap[x, y].ToString();

                state += string.Join(";", row);

            }

            state += "\nMOBMAP\n";
            foreach (Entity e in map.entities) state += e.typeString + "@-@" + e.X + " " + e.Y + "\n";

            return Encoding.UTF8.GetBytes(state);

        }

    }

}