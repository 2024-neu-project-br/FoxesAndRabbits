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
                            entitiesToBeAdded = new List<Entity>(),
                            entitiesToBeRemoved = new List<Entity>();

        public Map(GameInstance instance, int width, int height) {

            this.instance = instance;

            WIDTH = width;
            HEIGHT = height;

            grassMap = new int[width, height];
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) grassMap[x, y] = 3;
            grassMapNew = grassMap;

        }

        public int GetWrappedX(int x) => ((x % WIDTH) + WIDTH) % WIDTH; // brain damage
        public int GetWrappedY(int y) => ((y % HEIGHT) + HEIGHT) % HEIGHT;

        public Entity? GetEntityAt(int x, int y) {

            foreach (Entity e in entities) if (e.X == GetWrappedX(x) && e.Y == GetWrappedY(y)) return e;
            return null;

        }

        public List<Entity> GetEntitiesAround(int x, int y) => GetEntitiesAround(x, y, 1);

        public List<Entity> GetEntitiesAround(int x, int y, int radius) {

            List<Entity> nearbyEntities = new List<Entity>();

            x = GetWrappedX(x);
            y = GetWrappedY(y);

            for (int dx = -1 * radius; dx <= 1 * radius; dx++)
                for (int dy = -1 * radius; dy <= 1 * radius; dy++) {

                    if (dx == 0 && dy == 0) continue;

                    Entity? e = GetEntityAt(GetWrappedX(x + dx), GetWrappedY(y + dy));
                    if (e == null) continue;

                    nearbyEntities.Add(e);

                }

            return nearbyEntities;

        }

        public int[] GetRandomEmptyCellAround(int x, int y, int radius) {

            /*
            
            
                !!!!!!!ACHTUNG BITTE!!!!!!!
            
                Warning: this can get stuck in an infinite loop if the entity cannot find an empty spot around itself
            
            
            */

            Random random = new Random();

            x = GetWrappedX(x);
            y = GetWrappedY(y);

            int RandomRange() => (int) Math.Ceiling(radius * random.NextDouble()) * random.NextDouble() > 0.5 ? 1 : -1;

            int candidateX = GetWrappedX(x + RandomRange());
            int candidateY = GetWrappedY(y + RandomRange());

            if (GetEntityAt(candidateX, candidateY) != null) return GetRandomEmptyCellAround(x, y, radius);
            return [candidateX, candidateY];

        }

        public void AddNewEntity(Entity entity) => entitiesToBeAdded.Add(entity);

        public void Update() {

            for (int x = 0; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    if (grassMap[x, y] < 3) grassMapNew[x, y]++;

            foreach (Entity e in entities) e.Update();


        }

        public void Tick() {

            grassMap = grassMapNew;

            entities.AddRange(entitiesToBeAdded);
            foreach (Entity e in entitiesToBeRemoved) entities.Remove(e);

            entitiesToBeAdded.Clear();
            entitiesToBeRemoved.Clear();

            foreach (Entity e in entities) e.Tick();

        }

    }

}