using game.model.localmap;
using types;
using UnityEngine;

namespace game.model.util.validation {
    public class BuildingValidator : PositionValidator {

        public override bool validate(int x, int y, int z) {
            LocalMap map = GameModel.localMap;
            return map.passageMap.getPassage(x, y, z) == PassageTypes.PASSABLE.VALUE 
                   && map.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE;
        }

        public bool validateArea(Vector3Int position, Vector2Int size) {
            LocalMap map = GameModel.localMap;
            for (int x = position.x; x < position.x + size.x; x++) {
                for (int y = position.y; y < position.y + size.y; y++) {
                    if (map.passageMap.getPassage(x, y, position.z) != PassageTypes.PASSABLE.VALUE
                        || map.blockType.get(x, y, position.z) != BlockTypes.FLOOR.CODE)
                        return false;
                }
            }
            return true;
        }
    }
}