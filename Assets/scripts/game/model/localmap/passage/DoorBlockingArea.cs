using UnityEngine;
using static types.PassageTypes;

namespace game.model.localmap.passage {
public class DoorBlockingArea : AbstractArea {
    public DoorBlockingArea(LocalMap map, PassageMap passage) : base(map, passage) { }

    protected override bool tileCanHaveArea(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE;
    }

    protected override AbstractAreaUpdater createUpdater() {
        return new DoorBlockingAreaUpdater(this, map, passage);
    }

    protected override AbstractAreaInitializer createInitializer() {
        return new DoorBlockingAreaInitializer();
    }

    private class DoorBlockingAreaInitializer : AbstractAreaInitializer {
        protected override bool tilePassable(int x, int y, int z) {
            return passage.getPassage(x, y, z) == PASSABLE.VALUE;
        }
    }

    private class DoorBlockingAreaUpdater : AbstractAreaUpdater {
        public DoorBlockingAreaUpdater(AbstractArea area, LocalMap localMap, PassageMap passageMap) : base(area, localMap, passageMap) { }

        protected override bool tileCanHaveArea(int x, int y, int z) {
            return passage.getPassage(x, y, z) == PASSABLE.VALUE;
        }

        protected override bool hasPathBetweenNeighbours(Vector3Int pos1, Vector3Int pos2) {
            return passage.hasPathBetweenNeighbours(pos1, pos2);
        }
    }
}
}