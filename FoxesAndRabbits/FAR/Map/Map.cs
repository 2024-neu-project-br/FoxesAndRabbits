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
        public Entity?[,] futureOccupancyMap;

        public List<Entity> entities = new List<Entity>(),
                            entitiesToBeAdded = new List<Entity>(),
                            entitiesToBeRemoved = new List<Entity>();

        public Map(GameInstance instance, int width, int height) {

            this.instance = instance;

            WIDTH = width;
            HEIGHT = height;

            grassMap = new int[width, height];
            futureOccupancyMap = new Entity[width, height];

            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) {
                
                futureOccupancyMap[x, y] = null;
                grassMap[x, y] = 3;

            }

            grassMapNew = grassMap;

        }

        public int GetWrappedX(int x) => ((x % WIDTH) + WIDTH) % WIDTH; // brain damage
        public int GetWrappedY(int y) => ((y % HEIGHT) + HEIGHT) % HEIGHT;

        public Entity? GetEntityAt(int x, int y) {

            x = GetWrappedX(x);
            y = GetWrappedY(y);

            foreach (Entity e in entities) if (e.X == x && e.Y == y) return e;
            return null;

        }

        public List<int[]> GetCellsAround(int x, int y) => GetCellsAround(x, y, 1);

        public List<int[]> GetCellsAround(int x, int y, int radius) {

            List<int[]> cells = new List<int[]>();

            x = GetWrappedX(x);
            y = GetWrappedY(y);

            for (int dx = -radius; dx <= radius; dx++)
                for (int dy = -radius; dy <= radius; dy++) {

                    if (dx == 0 && dy == 0) continue;
                    cells.Add([GetWrappedX(x + dx), GetWrappedY(y + dy)]);

                }

            return cells;

        }

        public List<int[]> GetEmptyCellsAround(int x, int y) => GetEmptyCellsAround(x, y, 1);

        public List<int[]> GetEmptyCellsAround(int x, int y, int radius) {

            List<int[]> cells = GetCellsAround(x, y, radius), emptyCells = new List<int[]>();

            foreach (int[] cell in cells) if (GetEntityAt(cell[0], cell[1]) == null) emptyCells.Add(cell);

            return emptyCells;

        }

        public List<Entity> GetEntitiesAround(int x, int y) => GetEntitiesAround(x, y, 1);

        public List<Entity> GetEntitiesAround(int x, int y, int radius) {

            List<int[]> cellsInRange = GetCellsAround(x, y, radius);
            List<Entity> entitiesInRange = new List<Entity>();

            Entity? e;
            foreach (int[] cell in cellsInRange) if ((e = GetEntityAt(cell[0], cell[1])) != null) entitiesInRange.Add(e);

            return entitiesInRange;

        }

        public int[] GetRandomEmptyCellAround(int x, int y) => GetRandomEmptyCellAround(x, y, 1);

        public int[] GetRandomEmptyCellAround(int x, int y, int radius) {

            List<int[]> emptyCells = GetEmptyCellsAround(x, y, radius);
            
            if (emptyCells.Count == 0) return [GetWrappedX(x), GetWrappedY(y)]; // im not sure why im wrapping, maybe just so that i foolproof myself
            return emptyCells[instance.random.Next(emptyCells.Count)];

        }

        public void AddNewEntity(Entity entity) => entitiesToBeAdded.Add(entity);
        public void RemoveEntity(Entity entity) => entitiesToBeRemoved.Add(entity);

        int grassTick = 0;
        public void Update() {

            if (grassTick >= 2) {

                for (int x = 0; x < WIDTH; x++)
                    for (int y = 0; y < HEIGHT; y++)
                        if (grassMap[x, y] < 3) grassMapNew[x, y]++;
                grassTick = 0;

            }
            grassTick++;

            foreach (Entity e in entities) e.Update();


        }

        public void Tick() {

            grassMap = grassMapNew;
            for (int x = 0; x < WIDTH; x++) for (int y = 0; y < HEIGHT; y++) futureOccupancyMap[x, y] = null;

            entities.AddRange(entitiesToBeAdded);
            foreach (Entity e in entitiesToBeRemoved) entities.Remove(e);

            entitiesToBeAdded.Clear();
            entitiesToBeRemoved.Clear();

            foreach (Entity e in entities) e.Tick();

        }

    }

}