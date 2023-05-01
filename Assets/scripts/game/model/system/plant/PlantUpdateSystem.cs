using game.model.component.plant;
using generation.plant;
using Leopotam.Ecs;

namespace game.model.system.plant {
    // updates plant entity when PlantUpdateComponent added
    public class PlantUpdateSystem : LocalModelScalableEcsSystem {
        public EcsFilter<PlantUpdateComponent> filter;
        private readonly PlantGenerator generator = new();

        protected override void runLogic(int ticks) {
            foreach (int i in filter) {

            }
        }
    }
}