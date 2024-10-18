using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display;

namespace FoxesAndRabbits.FAR.Game {

    public class Game {
        
        public static List<GameInstance> instances = new List<GameInstance>();

        public static GameInstance GetInstance(string name) {

            foreach (GameInstance i in instances) if (i.name == name) return i;
#pragma warning disable CS8603 // Possible null reference return. AKA = shut the fuck up
            return null;
#pragma warning restore CS8603 // Possible null reference return.

        }

    }

}