using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display;
using FoxesAndRabbits.FAR.Entities;

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

            Random random = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (random.NextDouble() > 0.95) map.AddNewEntity(random.NextDouble() > 0.5 ? new Fox(this, [x, y]) : new Rabbit(this, [x, y]));

            Tick(); // this here is to push all new entities into the main list

        }

        public void Update() => map.Update();

        public void Tick() => map.Tick();

        public string State() {

            string state = "\nGRASSMAP\n";
            for (int y = 0; y < height; y++) {

                string[] row = new string[width];
                for (int x = 0; x < width; x++) row[x] = map.grassMap[x, y].ToString();

                state += string.Join(";", row) + "\n";

            }

            state += "\nMOBMAP\n";
            foreach (Entity e in map.entities) state += e.typeString + "@-@" + e.X + " " + e.Y + "\n";

            //Console.WriteLine("creating new bitch:\n" + state);

            return state;

        }

    }

}