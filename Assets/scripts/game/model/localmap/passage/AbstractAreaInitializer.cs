using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util;
using util.geometry;
using util.lang.extension;

namespace game.model.localmap.passage {
// synonym - a set of areas that are connected with each other. exists only on area inialization step
public abstract class AbstractAreaInitializer {
    private AbstractPassageHelper helper;
    protected string name = "AreaInitializer";
    private LocalMap localMap;
    protected PassageMap passage;
    
    private HashSet<HashSet<ushort>> synonyms; // after initial numbers assigning, some areas will be connected. sets contain numbers of connected areas
    private Dictionary<ushort, ushort> areaMapping; // synonym values to synonym min
    private bool debug = false;

    protected abstract bool tilePassable(int x, int y, int z);

    public void initArea(AbstractPassageHelper helper, LocalMap map, PassageMap passage, UtilUshortArray area) {
        this.helper = helper;
        localMap = map;
        this.passage = passage;
        synonyms = new();
        areaMapping = new();
        initAreaNumbers(area);
        log("synonyms count: " + synonyms.Count);
        processSynonyms();
        log("synonyms count: " + synonyms.Count);
        applyMapping(area);
        log("synonyms count: " + synonyms.Count);
    }

    // Assigns initial area numbers to cells, generates synonyms.
    private void initAreaNumbers(UtilUshortArray areaArray) {
        byte areaNum = 1;
         localMap.bounds.iterate((x, y, z) => {
            if (tilePassable(x, y, z)) { // not wall or space
                log(x + " " + y + " " + z + " -------------------------------------------");
                HashSet<ushort> neighbours = getNeighbours(x, y, z, areaArray);
                if (neighbours.Count == 0) { // cell not yet connected to any area
                    log(" set to " + (areaNum));
                    areaArray.set(x, y, z, areaNum++); // set new area number to cell
                } else {
                    log("neighbours " + neighbours.Count + " values: " + neighbours.Select(n => n + " ").Aggregate((s1, s2) => s1 + s2));
                    if (neighbours.Count > 1) addSynonyms(neighbours); // multiple areas accessible, merge
                    areaArray.set(x, y, z, neighbours.First()); // set number of connected area to map
                }
            }
        });
    }

    // Maps values from synonyms to lowest value from synonym
    private void processSynonyms() {
        foreach (HashSet<ushort> synonym in synonyms) {
            log("processing synonym " + synonym.Count);
            ushort min = synonym.Min(); // select minimal number from synonym
            synonym.ToList().ForEach(value => areaMapping.Add(value, min));
        }
    }

    // Sets area numbers from mapping to passage map, so only minimal values from synonyms are used.
    // Also initializes cell counter for areas.
    private void applyMapping(UtilUshortArray areaArray) {
        ushort area;
        localMap.bounds.iterate((x, y, z) => {
            area = areaArray.get(x, y, z); // unmapped value
            if (area != 0) { // passable tile
                if (areaMapping.ContainsKey(area)) {
                    areaArray.set(x, y, z, areaMapping[area]); // set mapped value
                }
            }
        });
    }

    // Adds given set to synonyms. Combines synonyms, if already encountered.
    private void addSynonyms(HashSet<ushort> neighbours) {
        if (synonyms.Any(set => set.includesAll(neighbours))) return; // if new synonym is fully contained in another one, do nothing
        // synonyms, intersecting with new one
        List<HashSet<ushort>> intersecting = synonyms.Where(synonym => synonym.Intersect(neighbours).Any()).ToList();
        if (intersecting.Count == 0) { // just add new synonym
            synonyms.Add(neighbours);
        } else { // merge all found synonyms
            log("merging synonyms " + intersecting.Count);
            intersecting.ForEach(set => synonyms.Remove(set));
            HashSet<ushort> mergedSynonym = new();
            foreach (HashSet<ushort> set in intersecting) {
                mergedSynonym.UnionWith(set);
            }
            log("merged synonyms into " + mergedSynonym);
            synonyms.Add(mergedSynonym);
        }
    }

    // Returns area numbers of areas accessible from given position
    private HashSet<ushort> getNeighbours(int cx, int cy, int cz, UtilUshortArray areaArray) {
        HashSet<ushort> neighbours = new();
        if (tilePassable(cx, cy, cz)) {
            for (int x = cx - 1; x <= cx + 1; x++) {
                for (int y = cy - 1; y <= cy + 1; y++) {
                    for (int z = cz - 1; z <= cz + 1; z++) {
                        if (localMap.inMap(x, y, z) && areaArray.get(x, y, z) != 0) {
                            if (helper.hasPathBetweenNeighbours(x, y, z, cx, cy, cz))
                                neighbours.Add(areaArray.get(x, y, z));
                        }
                    }
                }
            }
        }
        return neighbours;
    }

    private void log(object message) {
        if (debug) Debug.Log($"[{name}]: {message}");
    }
}
}