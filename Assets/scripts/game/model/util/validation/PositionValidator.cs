using UnityEngine;

namespace game.model.util.validation {
    public abstract class PositionValidator {
        public bool validate(Vector3Int pos, LocalModel model) => validate(pos.x, pos.y, pos.z, model);

        public abstract bool validate(int x, int y, int z, LocalModel model);
    }
}