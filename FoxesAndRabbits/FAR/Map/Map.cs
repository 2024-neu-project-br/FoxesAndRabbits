using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR {

    public class Map {

        public GameInstance instance;

        public int WIDTH, HEIGHT;

        public int[,] grassMap, grassMapNew;

        public List<Entity> entities = new List<Entity>(),
                            entitiesToBeAdded = new List<Entity>();

        public Map(GameInstance instance, int width, int height) {

            this.instance = instance;

            WIDTH = width;
            HEIGHT = height;

            grassMap = new int[width, height];
            grassMapNew = grassMap;

        }

        public int GetWrappedX(int x) => ((x % WIDTH) + WIDTH) % WIDTH; // brain damage
        public int GetWrappedY(int y) => ((y % HEIGHT) + HEIGHT) % HEIGHT;

        public Entity? GetEntityAt(int x, int y) {

            foreach (Entity e in entities) if (e.X == GetWrappedX(x) && e.Y == GetWrappedY(y)) return e;
            return null;

        }

        public List<Entity> GetEntitiesAround(int x, int y) {

            List<Entity> nearbyEntities = new List<Entity>();


            return nearbyEntities;

        }

    }

}