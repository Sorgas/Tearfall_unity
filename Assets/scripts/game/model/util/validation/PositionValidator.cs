using UnityEngine;

namespace game.model.util.validation {
    public abstract class PositionValidator {
        public bool validate(Vector3Int pos) => validate(pos.x, pos.y, pos.z);

        public abstract bool validate(int x, int y, int z);
    }
}