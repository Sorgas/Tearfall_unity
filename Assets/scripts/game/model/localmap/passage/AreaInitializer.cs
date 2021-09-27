using System.Collections.Generic;
using System.Linq;
using Assets.scripts.util.lang;
using UnityEngine;
using static Assets.scripts.enums.PassageEnum;

namespace Assets.scripts.game.model.localmap.passage {
    // synonym - a set of areas that are connected with each other. exists only on area inialization step
    class AreaInitializer {
        private LocalMap localMap;
        private PassageMap passage;
        private HashSet<HashSet<byte>> synonyms; // sets contain numbers of connected areas
        private Dictionary<byte, byte> areaMapping; // synonym values to synonym min

        public AreaInitializer(LocalMap localMap) {
            this.localMap = localMap;
        }

        // Creates {@link PassageMap} based on localMap.
        public void formPassageMap(PassageMap passage) {
            this.passage = passage;
            synonyms = new HashSet<HashSet<byte>>();
            areaMapping = new Dictionary<byte, byte>();
            initAreaNumbers();
            Debug.Log(synonyms.Count);
            processSynonyms();
            Debug.Log(synonyms.Count);
            applyMapping();
            Debug.Log(synonyms.Count);
        }

        // Assigns initial area numbers to cells, generates synonyms.
        private void initAreaNumbers() {
            byte areaNum = 1;
            for (int x = 0; x < localMap.xSize; x++) {
                for (int y = 0; y < localMap.ySize; y++) {
                    for (int z = 0; z < localMap.zSize; z++) {
                        if (passage.getPassage(x, y, z) == PASSABLE.VALUE) { // not wall or space
                            HashSet<byte> neighbours = getNeighbours(x, y, z);
                            if (neighbours.Count == 0) { // new area found
                                passage.area.set(x, y, z, areaNum++); // set new area number to map
                            } else {
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
                Debug.Log("processing synonym");
                byte min = synonym.Min(); // select minimal number from synonym
                synonym.ToList().ForEach(value => areaMapping.Add(value, min));
            }
        }

        // Sets area numbers from mapping to passage map, so only minimal values from synonyms are used.
        // Also initializes cell counter for areas.
        private void applyMapping() {
            byte area;
            for (int x = 0; x < localMap.xSize; x++) {
                for (int y = 0; y < localMap.ySize; y++) {
                    for (int z = 0; z < localMap.zSize; z++) {
                        area = passage.area.get(x, y, z); // unmapped value
                        if (area != 0) { // passable tile
                            if(areaMapping.ContainsKey(area)) {
                                passage.area.set(x, y, z, areaMapping[area]); // set mapped value
                            }
                        }
                    }
                }
            }

            foreach (byte i in passage.area.numbers.Keys) {
                Debug.Log(i + " " + passage.area.numbers[i]);
            }

        }

        /**
         * Adds given set to synonyms. Combines synonyms, if already encountered.
         * Returns first number from synonym.
         */
        private void addSynonyms(HashSet<byte> neighbours) {
            if(synonyms.Any(set => set.includesAll(neighbours))) return; // if new synonym is fully contained in another one, do nothing
            // synonyms, intersecting with new one
            List<HashSet<byte>> intersecting = synonyms.Where(synonym => synonym.Intersect(neighbours).Any()).ToList();
            // merge all found synonyms
            intersecting.ForEach(set => synonyms.Remove(set));
            HashSet<byte> mergedSynonym = new HashSet<byte>();
            foreach(HashSet<byte> set in intersecting) {
                mergedSynonym.UnionWith(set);
            }
            synonyms.Add(mergedSynonym);
        }

        /**
         * Returns area numbers of areas accessible from given position.
         * Does not return unset areas (0).
         */
        private HashSet<byte> getNeighbours(int cx, int cy, int cz) {
            HashSet<byte> neighbours = new HashSet<byte>();
            if (passage.getPassage(cx, cy, cz) != PASSABLE.VALUE) return neighbours; // impassable sell cannot have neigbours
            for (int x = cx - 1; x < cx + 2; x++) {
                for (int y = cy - 1; y < cy + 2; y++) {
                    for (int z = cz - 1; z < cz + 2; z++) {
                        if (passage.hasPathBetweenNeighbours(x, y, z, cx, cy, cz))
                            neighbours.Add(passage.area.get(x, y, z));
                    }
                }
            }
            neighbours.Remove(0);
            return neighbours;
        }
    }
}
