using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang.extension;
using static types.PassageTypes;

namespace game.model.localmap.passage {
    // synonym - a set of areas that are connected with each other. exists only on area inialization step
    class AreaInitializer {
        private LocalMap localMap;
        private PassageMap passage;
        private HashSet<HashSet<byte>> synonyms; // sets contain numbers of connected areas
        private Dictionary<byte, byte> areaMapping; // synonym values to synonym min
        private bool debugMode = false;

        public AreaInitializer(LocalMap localMap) {
            this.localMap = localMap;
        }

        public void formPassageMap(PassageMap passage) {
            this.passage = passage;
            synonyms = new();
            areaMapping = new();
            initAreaNumbers();
            log("synonyms count: " + synonyms.Count);
            processSynonyms();
            log("synonyms count: " + synonyms.Count);
            applyMapping();
            log("synonyms count: " + synonyms.Count);
        }

        // Assigns initial area numbers to cells, generates synonyms.
        private void initAreaNumbers() {
            byte areaNum = 1;
            for (int x = 0; x < localMap.bounds.maxX; x++) {
                for (int y = 0; y < localMap.bounds.maxY; y++) {
                    for (int z = 0; z < localMap.bounds.maxZ; z++) {
                        if (passage.getPassage(x, y, z) == PASSABLE.VALUE) { // not wall or space
                            log(x + " " + y + " " + z + " -------------------------------------------");
                            HashSet<byte> neighbours = getNeighbours(x, y, z);
                            if (neighbours.Count == 0) { // cell not yet connected to any area
                                log(" set to " + (areaNum));
                                passage.area.set(x, y, z, areaNum++); // set new area number to cell
                            } else {
                                log("neighbours " + neighbours.Count + " values: "+ neighbours.Select(n => n + " ").Aggregate((s1 , s2) => s1 + s2));
                                if (neighbours.Count > 1) addSynonyms(neighbours); // multiple areas accessible, merge
                                passage.area.set(x, y, z, neighbours.First()); // set number of connected area to map
                            }
                        }
                    }
                }
            }
        }

        // Maps values from synonyms to lowest value from synonym
        private void processSynonyms() {
            foreach (HashSet<byte> synonym in synonyms) {
                log("processing synonym " + synonym.Count);
                byte min = synonym.Min(); // select minimal number from synonym
                synonym.ToList().ForEach(value => areaMapping.Add(value, min));
            }
        }

        // Sets area numbers from mapping to passage map, so only minimal values from synonyms are used.
        // Also initializes cell counter for areas.
        private void applyMapping() {
            byte area;
            for (int x = 0; x < localMap.bounds.maxX; x++) {
                for (int y = 0; y < localMap.bounds.maxY; y++) {
                    for (int z = 0; z < localMap.bounds.maxZ; z++) {
                        area = passage.area.get(x, y, z); // unmapped value
                        if (area != 0) { // passable tile
                            if (areaMapping.ContainsKey(area)) {
                                passage.area.set(x, y, z, areaMapping[area]); // set mapped value
                            }
                        }
                    }
                }
            }

            foreach (byte i in passage.area.sizes.Keys) {
                log("area: " + i + ", size: " + passage.area.sizes[i]);
            }
        }

        // Adds given set to synonyms. Combines synonyms, if already encountered.
        private void addSynonyms(HashSet<byte> neighbours) {
            if (synonyms.Any(set => set.includesAll(neighbours))) return; // if new synonym is fully contained in another one, do nothing
            // synonyms, intersecting with new one
            List<HashSet<byte>> intersecting = synonyms.Where(synonym => synonym.Intersect(neighbours).Any()).ToList();
            if (intersecting.Count == 0) { // just add new synonym
                synonyms.Add(neighbours);
            } else { // merge all found synonyms
                log("merging synonyms " + intersecting.Count);
                intersecting.ForEach(set => synonyms.Remove(set));
                HashSet<byte> mergedSynonym = new();
                foreach (HashSet<byte> set in intersecting) {
                    mergedSynonym.UnionWith(set);
                }
                log("merged synonyms into " + mergedSynonym);
                synonyms.Add(mergedSynonym);
            }
        }

        // Returns area numbers of areas accessible from given position
        private HashSet<byte> getNeighbours(int cx, int cy, int cz) {
            HashSet<byte> neighbours = new();
            if (passage.getPassage(cx, cy, cz) != PASSABLE.VALUE) return neighbours; // impassable sell cannot have neigbours
            for (int x = cx - 1; x <= cx + 1; x++) {
                for (int y = cy - 1; y <= cy + 1; y++) {
                    for (int z = cz - 1; z <= cz + 1; z++) {
                        if (localMap.inMap(x, y, z) && passage.area.get(x, y, z) != 0) {
                            if (passage.hasPathBetweenNeighbours(x, y, z, cx, cy, cz))
                                neighbours.Add(passage.area.get(x, y, z));
                        }
                    }
                }
            }
            return neighbours;
        }

        private void log(object message) {
            if (debugMode) Debug.Log("[AreaInitializer]: " + message);
        }
    }
}