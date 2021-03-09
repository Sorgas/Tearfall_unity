using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums;
using Assets.scripts.util.extension;
using Assets.scripts.util.geometry;
using Assets.scripts.util.pathfinding;
using UnityEngine;
using static Assets.scripts.enums.PassageEnum;

namespace Assets.scripts.game.model.localmap.passage {
    public class PassageUpdater {
        private LocalMap map;
        private PassageMap passage;
        private AStar aStar;

        public PassageUpdater(LocalMap map, PassageMap passage) {
            this.map = map;
            this.passage = passage;
            aStar = GameModel.get<AStar>();
        }


        // Called when local map passage is updated. If cell becomes non-passable, it may split area into two.
        public void update(int x, int y, int z) {
            IntVector3 center = new IntVector3(x, y, z);
            Passage passing = passage.calculateTilePassage(center);
            passage.passage.set(center, passing.VALUE);
            if (passing == PASSABLE) { // tile became passable, areas should be merged
                List<byte> areas = new NeighbourPositionStream(center)
                        .filterConnectedToCenter()
                        .filterNotInArea(0)
                        .stream.Select(position => passage.area.get(position))
                        .ToList();
                // take new area number, if new tile is not connected to any area
                byte areaNumber = areas.Count() == 0 ? getUnusedAreaNumber() : areas.First();
                passage.area.set(x, y, z, areaNumber); // set area value to current tile
                if (areas.Count() > 1) mergeAreas(areas);
            } else { // tile became impassable, areas may split

                splitAreas(center);
            }
        }

        /**
     * Merges all given areas into one, keeping number of largest one.
     */
        private void mergeAreas(List<byte> areas) {
            //        Logger.PATH.logDebug("Merging areas " + areas);
            if (areas.Count() == 0) return;
            byte largestArea = passage.area.numbers.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            areas.Remove(largestArea);
            for (int x = 0; x < map.xSize; x++) {
                for (int y = 0; y < map.ySize; y++) {
                    for (int z = 0; z < map.zSize; z++) {
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
        private void splitAreas(IntVector3 center) {
            Dictionary<byte, List<IntVector3>> areas = collectAreasAround(center);
            UnityEngine.Debug.Log("Splitting areas around " + center + " in positions " + areas);
            foreach (byte areaValue in areas.Keys) {
                List<IntVector3> posList = areas[areaValue];
                if (posList.Count() < 2) continue; // single tile area
                List<HashSet<IntVector3>> isolatedPositions = collectIsolatedPositions(posList);
                if (isolatedPositions.Count() < 2) continue; // all positions from old areas remain connected, do nothing.
                isolatedPositions.RemoveAt(0);
                int oldCount = passage.area.numbers[areaValue];
                foreach (HashSet<IntVector3> positions in isolatedPositions) {
                    oldCount -= fill(positions.First(), getUnusedAreaNumber()); // refill isolated area with new number
                }
                // if (passage.area.numbers.get(areaValue).value != oldCount) Logger.PATH.logWarn("Areas sizes inconsistency after split.");
            }
        }

        // collects passable tiles around given one, groups them by area, and collects in dictionary
        private Dictionary<byte, List<IntVector3>> collectAreasAround(IntVector3 center) {
            return new NeighbourPositionStream(center)
                        .filterByPassage(PASSABLE)
                        .stream.ToDictionary(position => passage.area.get(position),
                        position => new List<IntVector3>() { position },
                        (l, r) => { l.AddRange(r); return l; });
        }

        // splits given list of positions into groups. positions in one group are interconnected. positions in different groups are isolated.
        private List<HashSet<IntVector3>> collectIsolatedPositions(List<IntVector3> list) {
            List<HashSet<IntVector3>> groups = new List<HashSet<IntVector3>>();
            while (list.Count() > 0) {
                HashSet<IntVector3> connectedPositions = new HashSet<IntVector3>();
                IntVector3 first = list.RemoveAndGet(0); // first position is connected to itself
                connectedPositions.Add(first);
                for (int i = list.Count() - 1; i >= 0; i--) {
                    IntVector3 pos = list[i];
                    // positions are accessible neighoburs or path exists
                    if (pos.isNeighbour(first) && passage.hasPathBetweenNeighbours(pos, first) || aStar.makeShortestPath(pos, first) != null) {
                        connectedPositions.Add(list.RemoveAndGet(i));
                    }
                }
                groups.Add(connectedPositions);
            }
            return groups;
        }

        // Fills all tiles available from given with new area value.
        private int fill(IntVector3 start, byte value) {
            int counter = 0;
            HashSet<IntVector3> openSet = new HashSet<IntVector3>();
            for (openSet.Add(start); openSet.Count() != 0; counter++) {
                IntVector3 center = openSet.First();
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
    }
}
