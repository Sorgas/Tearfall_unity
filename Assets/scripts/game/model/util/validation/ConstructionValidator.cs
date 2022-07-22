using types.building;

namespace game.model.util.validation {
    public class ConstructionValidator : PositionValidator {

        public override bool validate(int x, int y, int z) {
            return false;
        }

        public bool validateForConstruction(int x, int y, int z, ConstructionType type) {
            return true;
        }
    }
}