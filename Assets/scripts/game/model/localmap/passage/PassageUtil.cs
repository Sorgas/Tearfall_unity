using types;
using UnityEngine;
using static types.BlockTypes;

namespace game.model.localmap.passage {
// Utility class for passage-related calcultations
public class PassageUtil {
    private readonly LocalMap localMap;
    private readonly PassageMap passage;
    
    public PassageUtil(LocalMap localMap) {
        this.localMap = localMap;
    }

    public bool hasPathBetweenNeighbours(Vector3Int target, Vector3Int position) => hasPathBetweenNeighbours(target.x, target.y, target.z, position.x, position.y, position.z);

    public bool hasPathBetweenNeighbours(int tx, int ty, int tz, int x, int y, int z) {
        if (!localMap.inMap(tx, ty, tz) || !localMap.inMap(x, y, z) || passage.getPassage(x, y, z) == PassageTypes.IMPASSABLE.VALUE) return false;
        if (tz == z) return true;
        BlockType fromType = localMap.blockType.getEnumValue(tx, ty, tz);
        BlockType toType = localMap.blockType.getEnumValue(x, y, z);
        BlockType lowerType = tz < z ? fromType : toType;
        if (lowerType == RAMP) {
            // ramp has space above
            if (tx != x || ty != y) return (tz < z ? localMap.blockType.get(tx, ty, tz + 1) : localMap.blockType.get(x, y, z + 1)) == SPACE.CODE;
        } else if (lowerType == STAIRS) {
            if (tx == x && ty == y) {
                BlockType upperType = tz > z ? fromType : toType;
                return (upperType == STAIRS || upperType == DOWNSTAIRS) && lowerType == STAIRS; // handle stairs
            }
        }
        return false;
    }
    
    public bool hasPathBetweenNeighboursWithOverride(Vector3Int target, Vector3Int position, BlockType type)
        => hasPathBetweenNeighboursWithOverride(target.x, target.y, target.z, position.x, position.y, position.z, type);
        
    // /**
    //  * Checks that unit, standing in position will have access (to dig, open a chest) to target tile.
    //  * Same Z-level tiles are always accessible.
    //  * Tiles are accessible vertically with stairs or ramps.
    //  */
    public bool hasPathBetweenNeighboursWithOverride(int tx, int ty, int tz, int x, int y, int z, BlockType targetType) {
        if (!localMap.inMap(tx, ty, tz) || !localMap.inMap(x, y, z) || passage.getPassage(x, y, z) == PassageTypes.IMPASSABLE.VALUE) return false;
        if (tz == z) return true;
        BlockType fromType = localMap.blockType.getEnumValue(x, y, z);
        BlockType lowerType = tz < z ? targetType : fromType;
        if (lowerType == RAMP) {
            // ramp has space above
            if (tx != x || ty != y)
                return (tz < z ? localMap.blockType.get(tx, ty, tz + 1) : localMap.blockType.get(x, y, z + 1)) == SPACE.CODE;
        } else if (lowerType == STAIRS) {
            if (tx == x && ty == y) {
                BlockType upperType = tz > z ? targetType : fromType;
                return (upperType == STAIRS || upperType == DOWNSTAIRS) && lowerType == STAIRS; // handle stairs
            }
        }
        return false;
    }
}
}