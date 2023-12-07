using UnityEngine;
using static types.PassageTypes;

namespace game.model.localmap.passage {
public class DefaultArea : AbstractArea {

    public DefaultArea(LocalMap map, PassageMap passage) : base(map, passage) { }

    protected override bool tileCanHaveArea(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE || passage.getPassage(x, y, z) == DOOR.VALUE;
    }

    protected override AbstractAreaUpdater createUpdater() {
        return new DefaultAreaUpdater(this, map, passage);
    }

    protected override AbstractAreaInitializer createInitializer() {
        return new DefaultAreaInitializer();
    }

    private class DefaultAreaInitializer : AbstractAreaInitializer {
        protected override bool tilePassable(int x, int y, int z) {
            return passage.getPassage(x, y, z) == PASSABLE.VALUE || passage.getPassage(x, y, z) == DOOR.VALUE;
        }
    }

    private class DefaultAreaUpdater : AbstractAreaUpdater {
        public DefaultAreaUpdater(AbstractArea area, LocalMap localMap, PassageMap passageMap) : base(area, localMap, passageMap) { }

        protected override bool tileCanHaveArea(int x, int y, int z) {
            return passage.getPassage(x, y, z) == PASSABLE.VALUE || passage.getPassage(x, y, z) == DOOR.VALUE;
        }

        protected override bool hasPathBetweenNeighbours(Vector3Int pos1, Vector3Int pos2) {
            return passage.hasPathBetweenNeighbours(pos1, pos2);
        }
    }
}
}