using game.model.component.plant;
using Leopotam.Ecs;
using static game.model.component.plant.PlantUpdateType;

namespace game.model.system.plant {
    // updates plant entity when PlantUpdateComponent added
    public class PlantUpdateSystem : IEcsRunSystem {
        public EcsFilter<PlantUpdateComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                PlantUpdateType type = filter.Get1(i).type;
                if (type == GROW) {
                    // add PlantProductComponent
                    // add harvestable component
                }
            }
        }
    }
}