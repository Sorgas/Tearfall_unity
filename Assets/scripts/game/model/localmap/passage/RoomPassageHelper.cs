using static types.BlockTypes;
using static types.PassageTypes;

namespace game.model.localmap.passage {
// helper for counting rooms. counts doors as impassable tiles
public class RoomPassageHelper : AbstractPassageHelper {
    public RoomPassageHelper(PassageMap passage, LocalMap map) : base(passage, map) { }
    
    public override bool tileCanHaveArea(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE;
    }

    public override bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
        if (!map.inMap(x1, y1, z1) || !map.inMap(x2, y2, z2)) return false;
        int passageValue = passage.getPassage(x1, y1, z1);
        if(passageValue != PASSABLE.VALUE) return false;
        passageValue = passage.getPassage(x2, y2, z2); 
        if(passageValue != PASSABLE.VALUE) return false;
        if (z1 == z2) return true; // passable tiles on same level
        int type1 = map.blockType.get(x1, y1, z1);
        int type2 = map.blockType.get(x2, y2, z2);
        int lowerType = z1 < z2 ? type1 : type2;
        if (lowerType == RAMP.CODE) {
            if (x1 != x2 || y1 != y2) // not same xy 
                return (z1 < z2 ? map.blockType.get(x1, y1, z1 + 1) : map.blockType.get(x2, y2, z2 + 1)) == SPACE.CODE; // ramp has space above
        } else if (lowerType == STAIRS.CODE) {
            // stairs have stairs above
            if (x1 == x2 && y1 == y2) {
                int upper = z1 > z2 ? type1 : type2;
                return (upper == STAIRS.CODE || upper == DOWNSTAIRS.CODE) && lowerType == STAIRS.CODE; // handle stairs
            }
        }
        return false;
    }
}
}