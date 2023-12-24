using static types.BlockTypes;
using static types.PassageTypes;

namespace game.model.localmap.passage {
public class IndoorPassageHelper : AbstractPassageHelper {
    public IndoorPassageHelper(PassageMap passage, LocalMap map) : base(passage, map) { }

    public override bool tileCanHaveArea(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE || passage.getPassage(x, y, z) == FLY.VALUE;
    }

    public override bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
        if (!map.inMap(x1, y1, z1) || !map.inMap(x2, y2, z2)) return false;
        int passageValue = passage.getPassage(x1, y1, z1);
        if (passageValue != PASSABLE.VALUE && passageValue != FLY.VALUE) return false;
        passageValue = passage.getPassage(x2, y2, z2);
        if (passageValue != PASSABLE.VALUE && passageValue != FLY.VALUE) return false;
        if (z1 == z2) return true; // passable tiles on same level
        if (x1 == x2 && y1 == y2) { // tiles above each other
            int type1 = map.blockType.get(x1, y1, z1);
            int type2 = map.blockType.get(x2, y2, z2);
            int upperType = z1 > z2 ? type1 : type2;
            if (upperType == DOWNSTAIRS.CODE || upperType == SPACE.CODE) return true;
            if (upperType == STAIRS.CODE && (z1 < z2 ? type1 : type2) == STAIRS.CODE) { // stairs above stairs work as downstairs
                return true;
            }
        } else {
            return (z1 < z2 ? map.blockType.get(x1, y1, z1 + 1) : map.blockType.get(x2, y2, z2 + 1)) == SPACE.CODE; // lower tile has space above
        }
        return false;
    }
}
}