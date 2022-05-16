using game.model.component.plant;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.util.validation {
    public class TreeExistsValidator : PositionValidator {
        public override bool validate(int x, int y, int z) {
            EcsEntity plant = GameModel.get().plantContainer.getPlant(x, y, z);
            return !plant.IsNull() && plant.take<PlantComponent>().type.isTree;
        }
    }
}