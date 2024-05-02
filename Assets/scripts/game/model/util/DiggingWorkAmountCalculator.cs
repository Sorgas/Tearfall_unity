using System;
using System.Collections.Generic;
using System.Data;
using game.model.localmap;
using types;
using types.material;
using UnityEngine;
using static types.BlockTypes;

// calculates work amount for digging action basing on digging type and tile material
namespace game.model.util {
    public class DiggingWorkAmountCalculator {
    
        public int getWorkAmount(Vector3Int target, DesignationType type, LocalModel model) {
            BlockType currentType = model.localMap.blockType.getEnumValue(target);
            switch (type.name) {
                case "dig": // 12 for wall
                    return getWorkForTileTransition(target, FLOOR, model);
                case "stairs": {
                    BlockType targetType = (currentType == WALL ? STAIRS : DOWNSTAIRS);
                    return getWorkForTileTransition(target, targetType, model);
                }
                case "ramp": // 2
                    return getWorkForTileTransition(target, RAMP, model) + getWorkForTileTransition(target + Vector3Int.forward, SPACE, model);
                case "channel": {
                    int workAmount = getWorkForTileTransition(target, SPACE, model);
                    Vector3Int rampPosition = target + Vector3Int.back;
                    if(model.localMap.blockType.get(rampPosition) == WALL.CODE) {
                        workAmount += getWorkForTileTransition(target + Vector3Int.back, RAMP, model);
                    }
                    return workAmount;
                }
                case "downstairs": {
                    int workAmount = 0;
                    if (currentType != STAIRS && currentType != DOWNSTAIRS) {
                        workAmount += getWorkForTileTransition(target, DOWNSTAIRS, model); 
                    }
                    Vector3Int stairsPosition = target + Vector3Int.back;
                    byte lowerType = model.localMap.blockType.get(stairsPosition);
                    if (lowerType == WALL.CODE || lowerType == RAMP.CODE) {
                        workAmount += getWorkForTileTransition(stairsPosition, STAIRS, model); 
                    }
                    return workAmount;                
                }
            }
            throw new ArgumentException("Trying to find digging work amount for non-diggable tile " + target);
        }

        private int getWorkForTileTransition(Vector3Int target, BlockType type, LocalModel model) {
            BlockType currentType = model.localMap.blockType.getEnumValue(target);
            int workAmount = type.OPENNESS - currentType.OPENNESS; // 12 for wall -> floor
            return workAmount * getMaterialWorkAmountModifier(target, model);
        }

        private int getMaterialWorkAmountModifier(Vector3Int position, LocalModel model) {
            int material = model.localMap.blockType.getMaterial(position);
            HashSet<string> tags = MaterialMap.get().material(material).tags;
            if (tags.Contains("ore")) {
                return 150;
            } else if (tags.Contains("stone")) {
                return 100;
            } else if (tags.Contains("soil")) {
                return 50;
            }
            throw new DataException("Trying to find digging work amount for non-diggable tile " + position);
        }
    }
}