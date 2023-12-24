using UnityEngine;
using util;
using util.pathfinding;

namespace game.model.localmap.passage {
// provides functionality for pathfinding and passage area storing and updating
public abstract class AbstractPassageHelper {
    public UtilByteArrayWithCounter area;
    public AStar aStar;
    private readonly AreaUpdater updater;
    private readonly AbstractAreaInitializer initializer;

    protected readonly PassageMap passage;
    protected readonly LocalMap map;

    protected AbstractPassageHelper(PassageMap passage, LocalMap map) {
        this.passage = passage;
        this.map = map;
        area = new UtilByteArrayWithCounter(map.sizeVector);
        aStar = new AStar(this);
        updater = new AreaUpdater(this, area, map, passage); // TODO make not abstract
        initializer = new AreaInitializerGroundPassableDoors();
        initializer.initArea(this, map, passage, area);
    }

    public abstract bool tileCanHaveArea(int x, int y, int z);

    // should return true if a creature can move from one tile to another, and false otherwize
    // should check if positions are inside map and are passable.
    // positions should be neighbours
    public abstract bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2);

    public bool hasPathBetweenNeighbours(Vector3Int pos1, Vector3Int pos2) =>
        hasPathBetweenNeighbours(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);

    private bool tileIsAccessibleFromArea(int tx, int ty, int tz, int areaValue) {
        if (!map.inMap(tx, ty, tz)) return false;
        if (area.get(tx, ty, tz) == areaValue) return true;
        // if passable and not in same area, then it is in different area
        if (!tileCanHaveArea(tx, ty, tz)) {
            for (int x = tx - 1; x < tx + 2; x++) {
                for (int y = ty - 1; y < ty + 2; y++) {
                    if ((x != tx || y != ty) && map.inMap(x, y, tz) && area.get(x, y, tz) == areaValue) return true;
                }
            }
        }
        return false;
    }

    public bool tileIsAccessibleFromArea(Vector3Int target, Vector3Int from) =>
        tileIsAccessibleFromArea(target.x, target.y, target.z, area.get(from));

    public int getArea(Vector3Int position) => area.get(position.x, position.y, position.z);

    public void update(Vector3Int position) => updater.update(position);
}
}