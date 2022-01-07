using enums;

namespace game.model.util.validation {
    public class DiggingChannelValidator : DiggingValidator {

        public DiggingChannelValidator() : base(BlockTypeEnum.SPACE) { }

        public override bool validate(int x, int y, int z) {
            return z > 0 && base.validate(x, y, z);
        }
    }
}