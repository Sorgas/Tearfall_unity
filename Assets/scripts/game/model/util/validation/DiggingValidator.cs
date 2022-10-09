using types;

namespace game.model.util.validation {
    public class DiggingValidator : PositionValidator {
        public readonly BlockType targetBlockType;

        public DiggingValidator(BlockType targetBlockType) {
            this.targetBlockType = targetBlockType;
        }

        public override bool validate(int x, int y, int z, LocalModel model) {
            BlockType block = model.localMap.blockType.getEnumValue(x, y, z);
            return targetBlockType.OPENNESS > block.OPENNESS;
        }
    }
}