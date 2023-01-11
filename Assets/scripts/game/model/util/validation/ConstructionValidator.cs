using game.model.localmap;
using types.building;

namespace game.model.util.validation {
    public class ConstructionValidator : PositionValidator {

        public override bool validate(int x, int y, int z, LocalModel model) {
            return false;
        }

        public bool validateForConstruction(int x, int y, int z, ConstructionType type, LocalModel model) {
            return true;
        }
    }
}