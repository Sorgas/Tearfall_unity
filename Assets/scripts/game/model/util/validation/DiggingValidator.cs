using enums;
using types;
using UnityEngine;

namespace game.model.util.validation {
    public class DiggingValidator : PositionValidator {
        public readonly BlockType targetBlockType;

        public DiggingValidator(BlockType targetBlockType) {
            this.targetBlockType = targetBlockType;
        }

        public override bool validate(int x, int y, int z) {
            BlockType block = GameModel.localMap.blockType.getEnumValue(x, y, z);
            return targetBlockType.OPENNESS > block.OPENNESS;
        }
    }
}