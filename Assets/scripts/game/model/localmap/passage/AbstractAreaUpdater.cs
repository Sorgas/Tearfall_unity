using System.Collections.Generic;
using System.Linq;
using types;
using UnityEngine;
using util.geometry;
using util.lang.extension;
using util.pathfinding;
using static types.PassageTypes;

namespace game.model.localmap.passage {
// recalculates areas after one tile is changed
public abstract class AbstractAreaUpdater {
    private readonly AbstractArea area;
    protected readonly LocalMap map;
    protected readonly PassageMap passage;
    public bool debug = true;

    public AbstractAreaUpdater(AbstractArea area, LocalMap localMap, PassageMap passageMap) {
        this.area = area;
        map = localMap;
        passage = passageMap;
    }

    // Called when local map passage is updated. If cell becomes non-passable, it may split area into two.
    public void update(Vector3Int position) {
        log("updating " + position);
        if (tileCanHaveArea(position.x, position.y, position.z)) { // tile became passable, areas should be merged
            mergeAreasAroundCenter(position);
        } else {
            area.set(position, 0);
            // tile became impassable, areas may split
            // if under new SPACE tile is RAMP, areas may join
            if (position.z > 0 && map.blockType.get(position) == BlockTypes.SPACE.CODE
                               && map.blockType.get(position + Vector3Int.back) == BlockTypes.RAMP.CODE) {
                mergeAreasAboveRamp(position);
            }
            splitAreas(position);
        }
    }

    protected abstract bool tileCanHaveArea(int x, int y, int z);

    protected abstract bool hasPathBetweenNeighbours(Vector3Int pos1, Vector3Int pos2);
    
    // observes areas around center, sets center position to one of them or new area. Then merges areas
    private void mergeAreasAroundCenter(Vector3Int center) {
        Debug.Log("merging areas " + center);
        HashSet<byte> areas = PositionUtil.all.Select(pos => center + pos)
            .Where(pos => hasPathBetweenNeighbours(pos, center))
            .Select(pos => area.get(pos)).ToHashSet();
        areas.Remove(0);
        // take new area number, if new tile is not connected to any area
        area.set(center, areas.Count == 0 ? getUnusedAreaNumber() : areas.First());
        mergeAreas(areas);
    }

    private void mergeAreasAboveRamp(Vector3Int center) {
        Debug.Log("merging areas above ramp " + center);
        HashSet<byte> areas = PositionUtil.allNeighbour.Select(pos => center + pos)
            .Where(pos => tileCanHaveArea(pos.x, pos.y, pos.z))
            .Select(pos => area.get(pos)).ToHashSet();
        areas.Add(area.get(center + Vector3Int.back));
        if (areas.Count > 1) mergeAreas(areas);
    }

    // sets all tiles of given areas to the largest one 
    private void mergeAreas(HashSet<byte> areas) {
        if (areas.Count < 2) return;
        byte largestArea = area.sizes
            .Where(pair => areas.Contains(pair.Key))
            .Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        areas.Remove(largestArea);
        for (int x = 0; x < map.bounds.maxX; x++) {
            for (int y = 0; y < map.bounds.maxY; y++) {
                for (int z = 0; z < map.bounds.maxZ; z++) {
                    if (areas.Contains(area.get(x, y, z))) {
                        area.set(x, y, z, largestArea);
                    }
                }
            }
        }
    }

    /**
     * Refills areas if they were split. Areas that were different before update cannot be merged.
     * Gets sets of tiles of same area and splits them into subsets of connected tiles.
     * If there were more than 1 subset(area has been split), refills such areas with new number.
     *
     * @param center - position which became impassable.
     */
    private void splitAreas(Vector3Int center) {
        log("splitting areas " + center);
        Dictionary<byte, List<Vector3Int>> areas = new NeighbourPositionStream(center, map).filterByPassage(PASSABLE).groupByAreas();
        foreach (byte areaValue in areas.Keys) {
            List<Vector3Int> posList = areas[areaValue];
            if (posList.Count < 2) continue; // single tile area
            List<HashSet<Vector3Int>> isolatedPositions = collectIsolatedPositions(posList);
            if (isolatedPositions.Count < 2) continue; // all positions from old areas remain connected, do nothing.
            isolatedPositions.RemoveAt(0);
            if (!area.sizes.ContainsKey(areaValue)) {
                Debug.Log("");
            }
            int oldCount = area.sizes[areaValue];
            foreach (HashSet<Vector3Int> positions in isolatedPositions) {
                oldCount -= fill(positions.First(), getUnusedAreaNumber()); // refill isolated area with new number
            }
            // if (area.numbers.get(areaValue).value != oldCount) Logger.PATH.logWarn("Areas sizes inconsistency after split.");
        }
    }

    // Splits given list of positions into groups. Positions in one group are interconnected. Positions in different groups are isolated.
    private List<HashSet<Vector3Int>> collectIsolatedPositions(List<Vector3Int> list) {
        List<HashSet<Vector3Int>> groups = new();
        while (list.Count > 0) {
            HashSet<Vector3Int> connectedPositions = new();
            Vector3Int first = list.removeAndGet(0); // first position is connected to itself
            connectedPositions.Add(first);
            for (int i = list.Count - 1; i >= 0; i--) {
                Vector3Int pos = list[i];
                if (pos.isNeighbour(first) && hasPathBetweenNeighbours(pos, first)
                    || AStar.get().pathExists(pos, first, map)) {
                    connectedPositions.Add(list.removeAndGet(i));
                }
            }
            groups.Add(connectedPositions);
        }
        return groups;
    }

    // Fills all tiles available from given with new area value.
    private int fill(Vector3Int start, byte value) {
        int counter = 0;
        HashSet<Vector3Int> openSet = new();
        for (openSet.Add(start); openSet.Count() != 0; counter++) {
            Vector3Int center = openSet.First();
            openSet.Remove(center);
            area.set(center.x, center.y, center.z, value);
            new NeighbourPositionStream(center, map)
                .filterConnectedToCenter()
                .filterNotInArea(value)
                .stream.ToList().ForEach(pos => openSet.Add(pos));
        }
        return counter;
    }

    private byte getUnusedAreaNumber() {
        for (byte i = 0; i < byte.MaxValue; i++)
            if (!area.sizes.Keys.Contains(i))
                return i;
        return 0;
    }

    private void log(string message) {
        if(debug) Debug.Log(message);
    }
}
}