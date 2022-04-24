using System.Collections.Generic;
using System.Linq;
using enums;
using UnityEngine;
using util.extension;
using util.geometry;
using util.pathfinding;
using static enums.PassageEnum;

namespace game.model.localmap.passage {
    public class PassageUpdater {
        private LocalMap map;
        private PassageMap passage;
        private bool debug = true;
        private string logMessage;
        
        public PassageUpdater(LocalMap map, PassageMap passage) {
            this.map = map;
            this.passage = passage;
        }
        
        // Called when local map passage is updated. If cell becomes non-passable, it may split area into two.
        public void update(int x, int y, int z) {
            log("updating passage in " + x + " " + y + " " + z);
            Vector3Int center = new Vector3Int(x, y, z);
            Passage passing = passage.calculateTilePassage(center);
            passage.passage.set(center, passing.VALUE);
            if (passing == PASSABLE) { // tile became passable, areas should be merged
                HashSet<byte> areas = new NeighbourPositionStream(center)
                        .filterConnectedToCenter()
                        .filterNotInArea(0)
                        .stream.Select(position => passage.area.get(position))
                        .ToHashSet();
                // take new area number, if new tile is not connected to any area
                byte areaNumber = areas.Count == 0 ? getUnusedAreaNumber() : areas.First();
                passage.area.set(x, y, z, areaNumber); // set area value to current tile
                if (areas.Count > 1) mergeAreas(areas);
            } else { // tile became impassable, areas may split
                splitAreas(center);
            }
            Debug.Log(logMessage);
            logMessage = "";
        }

        // sets all tiles of all areas to the largest one 
        private void mergeAreas(HashSet<byte> areas) {
            if (areas.Count == 0) return;
            byte largestArea = passage.area.numbers.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
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
         * @param posMap - area number to positions of this area.
         */
        private void splitAreas(Vector3Int center) {
            log("splitting areas");
            Dictionary<byte, List<Vector3Int>> areas = collectAreasAround(center);
            UnityEngine.Debug.Log("Splitting areas around " + center + " in positions " + areas);
            foreach (byte areaValue in areas.Keys) {
                List<Vector3Int> posList = areas[areaValue];
                if (posList.Count() < 2) continue; // single tile area
                List<HashSet<Vector3Int>> isolatedPositions = collectIsolatedPositions(posList);
                if (isolatedPositions.Count() < 2) continue; // all positions from old areas remain connected, do nothing.
                isolatedPositions.RemoveAt(0);
                int oldCount = passage.area.numbers[areaValue];
                foreach (HashSet<Vector3Int> positions in isolatedPositions) {
                    oldCount -= fill(positions.First(), getUnusedAreaNumber()); // refill isolated area with new number
                }
                // if (passage.area.numbers.get(areaValue).value != oldCount) Logger.PATH.logWarn("Areas sizes inconsistency after split.");
            }
        }

        // collects passable tiles around given one, groups them by area, and collects in dictionary
        private Dictionary<byte, List<Vector3Int>> collectAreasAround(Vector3Int center) {
            return new NeighbourPositionStream(center)
                        .filterByPassage(PASSABLE)
                        .stream.ToDictionary(position => passage.area.get(position),
                        position => new List<Vector3Int>() { position },
                        (l, r) => { l.AddRange(r); return l; });
        }

        // splits given list of positions into groups. positions in one group are interconnected. positions in different groups are isolated.
        private List<HashSet<Vector3Int>> collectIsolatedPositions(List<Vector3Int> list) {
            List<HashSet<Vector3Int>> groups = new List<HashSet<Vector3Int>>();
            while (list.Count() > 0) {
                HashSet<Vector3Int> connectedPositions = new HashSet<Vector3Int>();
                Vector3Int first = list.RemoveAndGet(0); // first position is connected to itself
                connectedPositions.Add(first);
                for (int i = list.Count() - 1; i >= 0; i--) {
                    Vector3Int pos = list[i];
                    // positions are accessible neighoburs or path exists
                    if (pos.isNeighbour(first) && passage.hasPathBetweenNeighbours(pos, first) || AStar.get().makeShortestPath(pos, first) != null) {
                        connectedPositions.Add(list.RemoveAndGet(i));
                    }
                }
                groups.Add(connectedPositions);
            }
            return groups;
        }

        // Fills all tiles available from given with new area value.
        private int fill(Vector3Int start, byte value) {
            int counter = 0;
            HashSet<Vector3Int> openSet = new HashSet<Vector3Int>();
            for (openSet.Add(start); openSet.Count() != 0; counter++) {
                Vector3Int center = openSet.First();
                openSet.Remove(center);
                passage.area.set(center.x, center.y, center.z, value);
                new NeighbourPositionStream(center)
                        .filterConnectedToCenter()
                        .filterNotInArea(value)
                        .stream.ToList().ForEach(pos => openSet.Add(pos));
            }
            return counter;
        }

        private byte getUnusedAreaNumber() {
            for (byte i = 0; i < byte.MaxValue; i++)
                if (!passage.area.numbers.Keys.Contains(i)) return i;
            return 0;
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}
