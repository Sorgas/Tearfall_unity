using enums;
using types;

namespace game.model.util.validation {
    public class DiggingChannelValidator : DiggingValidator {

        public DiggingChannelValidator() : base(BlockTypes.SPACE) { }

        public override bool validate(int x, int y, int z, LocalModel model) {
            return z > 0 && base.validate(x, y, z, model);
        }
    }
}