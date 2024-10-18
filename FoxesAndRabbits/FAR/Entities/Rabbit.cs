using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Entities {

    public class Rabbit : Entity {

        public Rabbit(GameInstance instance, int[] initialPos) : base(instance, EntityType.RABBIT, initialPos) {}
        
    }

}