using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums.plant;
using Assets.scripts.util.geometry;

namespace Assets.scripts.entity.plant {
    class AbstractPlant {
        public readonly PlantType type;

        protected AbstractPlant(IntVector3 position, PlantType type) {
            //base(position);
            this.type = type;
        }

        protected AbstractPlant(PlantType type) {
            //super();
            this.type = type;
        }

        public PlantLifeStage getCurrentLifeStage() {
            //return optional(PlantGrowthAspect.class).map(aspect -> type.lifeStages.get(aspect.stageIndex)).orElse(null);
            return null;
        }
    }
}
