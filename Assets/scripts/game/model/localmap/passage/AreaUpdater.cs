using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using util;
using util.geometry;
using util.lang;
using util.lang.extension;

namespace game.model.localmap.passage {
// recalculates areas after one tile is changed
public class AreaUpdater {
    private AbstractPassageHelper helper;
    private readonly UtilUshortArrayWithCounter area;
    private readonly LocalMap map;
    private readonly PassageMap passage;
    private readonly string name = "AbstractAreaUpdater";
    private readonly bool debug = true;
    public Action<IEnumerable<Vector3Int>> mergingCallback; // called when areas are merged.
    public Action<List<Vector3Int>> splittingCallback; // called when areas are merged.

    public AreaUpdater(AbstractPassageHelper helper, UtilUshortArrayWithCounter area, LocalMap localMap, PassageMap passageMap) {
        this.helper = helper;
        this.area = area;
        map = localMap;
        passage = passageMap;
    }

    // Called when local map passage is updated. If cell becomes non-passable, it may split area into two.
    public void update(Vector3Int position) {
        log("updating areas on position " + position);
        // all areas accessible from center should be merged
        if (helper.tileCanHaveArea(position.x, position.y, position.z)) { // tile became passable, areas should be merged
            mergeAreasAroundCenter(position);
        }
        splitAreas(position);
    }

    // observes areas around center, sets center position to one of them or new area. Then merges areas
    private void mergeAreasAroundCenter(Vector3Int center) {
        log($"merging areas {center}");
        // find positions in different areas, connected to center
        Dictionary<ushort, Vector3Int> positionsOfAreas = PositionUtil.all
            .Select(pos => center + pos)
            .Where(pos => helper.tileCanHaveArea(pos.x, pos.y, pos.z))
            .Where(pos => helper.hasPathBetweenNeighbours(pos, center))
            .ToDictionary(pos => helper.getArea(pos), pos => pos, (pos1, pos2) => pos1);
        // log("found areas to merge: " + setToString(areas));
        // take new area number, if new tile is not connected to any area
        if (positionsOfAreas.Count == 0) { // no tiles connected to center
            area.set(center, getUnusedAreaNumber()); // assign new area to isolated tile
            mergingCallback?.Invoke(new[] {center});
        } else {
            if (positionsOfAreas.Count == 1) {
                area.set(center, positionsOfAreas.Keys.First()); // extend existing area to center tile
            } else { // multiple areas became connected
                mergeAreas(Enumerable.ToHashSet(positionsOfAreas.Keys));
            }
            mergingCallback?.Invoke(positionsOfAreas.Values);
        }
    }

    // sets all tiles of given areas to the largest one 
    private void mergeAreas(HashSet<ushort> areas) {
        if (areas.Count < 2) return;
        log($"merging areas {setToString(areas)}");
        ushort largestArea = area.sizes
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
     * 
     * Refills areas if they were split. Areas that were different before update cannot be merged.
     * Gets sets of tiles of same area and splits them into subsets of connected tiles.
     * If there were more than 1 subset(area has been split), refills such areas with new number.
     *
     * @param center - position which became impassable.
     */
    private void splitAreas(Vector3Int center) {
        log("splitting areas " + center);
        MultiValueDictionary<ushort, Vector3Int> areas = new();
        foreach (var pos in PositionUtil.all
                     .Select(pos => center + pos)
                     .Where(pos => helper.tileCanHaveArea(pos.x, pos.y, pos.z))) {
            areas.add(area.get(pos), pos);
        }
        log($"areas found around center {areas.Count}");
        foreach (byte areaValue in areas.Keys) {
            List<Vector3Int> posList = areas[areaValue];
            if (posList.Count < 2) continue; // single tile area
            List<List<Vector3Int>> isolatedPositions = collectIsolatedPositions(posList, center);
            log($"isolated areas found for area {areaValue}: {isolatedPositions.Count}");
            if (isolatedPositions.Count < 2) continue; // all positions from old areas remain connected, do nothing.
            splittingCallback?.Invoke(isolatedPositions.Select(list => list.First()).ToList());
            isolatedPositions.RemoveAt(0);
            if (!area.sizes.ContainsKey(areaValue)) {
                Debug.LogError("area value not counted in area size");
            }
            int oldCount = area.sizes[areaValue];
            foreach (List<Vector3Int> positions in isolatedPositions) {
                oldCount -= fill(positions.First(), getUnusedAreaNumber()); // refill isolated area with new number
            }
            // if (area.numbers.get(areaValue).value != oldCount) Logger.PATH.logWarn("Areas sizes inconsistency after split.");
        }
        if (!helper.tileCanHaveArea(center.x, center.y, center.z)) {
            area.set(center, 0);
        }
    }

    // Splits given list of positions into groups. Positions in one group are interconnected.
    // Positions in different groups are not connected. List - positions around center having same area
    private List<List<Vector3Int>> collectIsolatedPositions(List<Vector3Int> list, Vector3Int center) {
        log($"collecting isolated positions around {center}");
        List<List<Vector3Int>> groups = new();
        while (list.Count > 0) {
            Vector3Int first = list.removeAndGet(0); // first position is connected to itself
            List<Vector3Int> connectedPositions = collectTransitiveNeighbours(list, first);
            log($"transitive neighbours collected {connectedPositions.Count}");
            for (int i = list.Count - 1; i >= 0; i--) {
                if (helper.aStar.pathExistsBiDirectional(list[i], connectedPositions[0])) {
                    connectedPositions.Add(list.removeAndGet(i));
                }
            }
            groups.Add(connectedPositions);
        }
        return groups;
    }

    // from given list collects positions which are accessible neighbours for start position, or position already collected from the list.
    // Removes collected positions from source list.
    private List<Vector3Int> collectTransitiveNeighbours(List<Vector3Int> list, Vector3Int start) {
        List<Vector3Int> result = new();
        result.Add(start);
        bool added = true;
        while (added && list.Count > 0) {
            added = false;
            for (var j = 0; j < result.Count; j++) { // forward loop, because elements added to result
                for (int i = list.Count - 1; i >= 0; i--) { // backward loop, because elements removed from list
                    // log($"i {i}, j {j}, list {list.Count}, result {result.Count}");
                    if (result[j].isNeighbour(list[i]) && helper.hasPathBetweenNeighbours(list[i], result[j])) {
                        result.Add(list.removeAndGet(i));
                        added = true;
                    }
                }
            }
        }
        return result;
    }

    // Fills all tiles available from given with new area value.
    private int fill(Vector3Int start, byte value) {
        log($"filling area from {start} with {value}");
        int counter = 0;
        HashSet<Vector3Int> openSet = new();
        for (openSet.Add(start); openSet.Count != 0; counter++) {
            Vector3Int center = openSet.First();
            openSet.Remove(center);
            area.set(center.x, center.y, center.z, value);
            PositionUtil.all.Select(pos => center + pos)
                .Where(pos => map.inMap(pos))
                .Where(pos => area.get(pos) != 0 && area.get(pos) != value)
                .Where(pos => helper.hasPathBetweenNeighbours(pos, center))
                .ForEach(pos => openSet.Add(pos));
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
        if (debug) Debug.Log($"[{name}]: {message}");
    }

    private string setToString(HashSet<ushort> set) {
        return string.Join(", ", set);
    }

    private string setToString(List<Vector3Int> set) {
        return "([" + string.Join("], [", set) + "])";
    }
}
}