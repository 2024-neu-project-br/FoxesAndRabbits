using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Fox : Entity {
        
        public Fox(GameInstance instance, int[] initialPos) : base(instance, EntityType.FOX, initialPos) {}

    }

}