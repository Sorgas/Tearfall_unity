using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.util.lang;
using static Assets.scripts.enums.PassageEnum;

namespace Assets.scripts.game.model.localmap.passage {
    // synonym - a set of areas that are connected with each other. exists only on area inialization step
    class AreaInitializer {
        private LocalMap localMap;
        private PassageMap passage;
        private HashSet<HashSet<byte>> connected; // sets contain numbers of connected areas
        private Dictionary<byte, byte> areaMapping; // synonym values to synonym min

        public AreaInitializer(LocalMap localMap) {
            this.localMap = localMap;
        }

        /**
         * Creates {@link PassageMap} based on localMap.
         */
        public void formPassageMap(PassageMap passage) {
            this.passage = passage;
            connected = new HashSet<HashSet<byte>>();
            areaMapping = new Dictionary<byte, byte>();
            initAreaNumbers();
            processSynonyms();
            applyMapping();
        }

        /**
         * Assigns initial area numbers to cells, generates synonyms.
         */
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
            foreach (HashSet<byte> synonym in connected) {
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
                            areaMapping.TryGetValue(area, out area);
                            passage.area.set(x, y, z, areaMapping[area]); // set mapped value
                            passage.area.numbers[area]++; // increment 
                        }
                    }
                }
            }
        }

        /**
         * Adds given set to synonyms. Combines synonyms, if already encountered.
         * Returns first number from synonym.
         */
        private void addSynonyms(HashSet<byte> neighbours) {
            // synonyms, intersecting with new one
            HashSet<HashSet<byte>> intersectingSynonyms = new HashSet<HashSet<byte>>();
            connected.Where(bytes => !bytes.Intersect(neighbours).Any())
                .ToList()
                .ForEach(bytes => intersectingSynonyms.Add(bytes));
            // if new synonym is fully contained in another one
            if (intersectingSynonyms.Count == 1 && EnumerableUtil.includesAll(intersectingSynonyms.First(), neighbours)) return;
            // merge all found synonyms
            connected.RemoveWhere(bytes => intersectingSynonyms.Contains(bytes));
            HashSet<byte> mergedSynonym = new HashSet<byte>();
            intersectingSynonyms.ToList().ForEach(bytes => mergedSynonym.UnionWith(bytes)); // merge intersecting synonyms
            mergedSynonym.UnionWith(neighbours);
            connected.Add(mergedSynonym);
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
