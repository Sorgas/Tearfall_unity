using System.Collections.Generic;
using System.Linq;
using game.model.container;
using types;
using UnityEngine;
using util.geometry;
using util.lang.extension;
using util.pathfinding;
using static types.PassageTypes;

namespace game.model.localmap.passage {
    public class PassageUpdater : LocalModelContainer {
        private LocalMap map;
        private PassageMap passage;
        private bool debug = true;
        private string logMessage;

        public PassageUpdater(LocalModel model, LocalMap localMap, PassageMap passageMap) : base(model) {
            map = localMap;
            passage = passageMap;
        }

        // Called when local map passage is updated. If cell becomes non-passable, it may split area into two.
        public void update(int x, int y, int z) {
            log("updating passage in " + x + " " + y + " " + z);
            Vector3Int center = new(x, y, z);
            Passage passing = passage.calculateTilePassage(center);
            passage.passage.set(center, passing.VALUE);
            if (passing == PASSABLE) { // tile became passable, areas should be merged
                mergeAreasAroundCenter(center);
            } else {
                passage.area.set(center, 0);
                // tile became impassable, areas may split
                // if under new SPACE tile is RAMP, areas may join
                if (z > 0 && map.blockType.get(center) == BlockTypes.SPACE.CODE
                          && map.blockType.get(center + Vector3Int.back) == BlockTypes.RAMP.CODE) {
                    mergeAreasAboveRamp(center);
                }
                splitAreas(center);
            }
            Debug.Log(logMessage);
            logMessage = "";
        }

        private void mergeAreasAroundCenter(Vector3Int center) {
            List<byte> areas = new NeighbourPositionStream(center, model).filterConnectedToCenter().collectAreas();
            // areas.Remove(0);
            // take new area number, if new tile is not connected to any area
            passage.area.set(center, areas.Count == 0 ? getUnusedAreaNumber() : areas.First());
            if (areas.Count > 1) mergeAreas(areas);
        }
        
        private void mergeAreasAboveRamp(Vector3Int center) {
            List<byte> areas = new NeighbourPositionStream(model).onSameZLevel(center).filterByPassage(PASSABLE).collectAreas();
            areas.Add(passage.area.get(center + Vector3Int.back));
            if (areas.Count > 1) mergeAreas(areas);
        }
        
        // sets all tiles of all areas to the largest one 
        private void mergeAreas(List<byte> areas) {
            if (areas.Count == 0) return;
            byte    largestArea = passage.area.sizes
                .Where(pair => areas.Contains(pair.Key))
                .Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            areas.Remove(largestArea);
            log("merging areas " + areas + " to area " + largestArea);
            for (int x = 0; x < map.bounds.maxX; x++) {
                for (int y = 0; y < map.bounds.maxY; y++) {
                    for (int z = 0; z < map.bounds.maxZ; z++) {
                        if (areas.Contains(passage.area.get(x, y, z))) {
                            passage.area.set(x, y, z, largestArea);
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
            Dictionary<byte, List<Vector3Int>> areas = new NeighbourPositionStream(center, model).filterByPassage(PASSABLE).groupByAreas();
            log("Splitting areas around " + center + " in positions " + areas);
            foreach (byte areaValue in areas.Keys) {
                List<Vector3Int> posList = areas[areaValue];
                if (posList.Count < 2) continue; // single tile area
                List<HashSet<Vector3Int>> isolatedPositions = collectIsolatedPositions(posList);
                if (isolatedPositions.Count < 2) continue; // all positions from old areas remain connected, do nothing.
                isolatedPositions.RemoveAt(0);
                if (!passage.area.sizes.ContainsKey(areaValue)) {
                    Debug.Log("");
                }
                int oldCount = passage.area.sizes[areaValue];
                foreach (HashSet<Vector3Int> positions in isolatedPositions) {
                    oldCount -= fill(positions.First(), getUnusedAreaNumber()); // refill isolated area with new number
                }
                // if (passage.area.numbers.get(areaValue).value != oldCount) Logger.PATH.logWarn("Areas sizes inconsistency after split.");
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
                    if (pos.isNeighbour(first) && passage.hasPathBetweenNeighbours(pos, first) 
                        || AStar.get().pathExists(pos, first, model.localMap)) {
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
                passage.area.set(center.x, center.y, center.z, value);
                new NeighbourPositionStream(center, model)
                    .filterConnectedToCenter()
                    .filterNotInArea(value)
                    .stream.ToList().ForEach(pos => openSet.Add(pos));
            }
            return counter;
        }

        private byte getUnusedAreaNumber() {
            for (byte i = 0; i < byte.MaxValue; i++)
                if (!passage.area.sizes.Keys.Contains(i))
                    return i;
            return 0;
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}