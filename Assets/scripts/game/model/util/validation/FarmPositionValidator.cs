using game.model.localmap;
using types;
using types.material;

namespace game.model.util.validation {
    public class FarmPositionValidator : PositionValidator {

        public override bool validate(int x, int y, int z, LocalModel model) {
            return model.localMap.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE 
                   && MaterialMap.get().material(model.localMap.blockType.getMaterial(x, y, z)).tags.Contains("soil");
        }
    }
}