using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;

namespace game.model.util.validation {
    // validates tiles for stockpile creation
    public class StockpilePositionValidator : PositionValidator {
        
        public override bool validate(int x, int y, int z, LocalModel model) {
            return model.localMap.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE 
                   && model.buildingContainer.getBuilding(new Vector3Int(x, y, z)) == EcsEntity.Null;
        }
    }
}