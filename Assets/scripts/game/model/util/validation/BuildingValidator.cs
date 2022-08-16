using game.model.localmap;
using types;
using UnityEngine;

namespace game.model.util.validation {
    public class BuildingValidator : PositionValidator {

        public override bool validate(int x, int y, int z) {
            LocalMap map = GameModel.localMap;
            return validate(x, y, z, map);
        }

        public bool validateArea(Vector3Int position, Vector2Int size) {
            LocalMap map = GameModel.localMap;
            for (int x = position.x; x < position.x + size.x; x++) {
                for (int y = position.y; y < position.y + size.y; y++) {
                    if (!validate(x, y, position.z, map)) return false;
                }
            }
            return true;
        }

        public bool validate(int x, int y, int z, LocalMap map) {
            return map.inMap(x, y, z)
                   && map.passageMap.getPassage(x, y, z) == PassageTypes.PASSABLE.VALUE
                   && map.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE;
        }
    }
}