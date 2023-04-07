using game.model.component.plant;
using Leopotam.Ecs;

namespace game.model.system.plant {
    public class PlantGrowingSystem : LocalModelScalableEcsSystem {
        public EcsFilter<PlantComponent>.Exclude<PlantRemoveComponent> filter;
        
        protected override void runLogic(int ticks) {
            foreach (int i in filter) {
                PlantComponent plant = filter.Get1(i);
                
            }
        }

        private void growPlant() {
            // if tile is lit
                // add growth
                
        }
    }
}