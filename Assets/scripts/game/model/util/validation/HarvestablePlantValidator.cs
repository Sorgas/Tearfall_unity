using game.model.component.plant;
using game.model.localmap;
using Leopotam.Ecs;

namespace game.model.util.validation {
    // validates that plant exists and has harvest component
    public class HarvestablePlantValidator : PositionValidator {

        public override bool validate(int x, int y, int z, LocalModel model) {
            EcsEntity plant = model.plantContainer.getPlant(x, y, z);
            return plant != EcsEntity.Null && plant.Has<PlantHarvestableComponent>();
        }
    }
}