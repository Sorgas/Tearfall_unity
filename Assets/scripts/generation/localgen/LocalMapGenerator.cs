using System.Collections.Generic;
using game.model.localmap;
using UnityEngine;

// generates LocalMap by world and location in that world. 
namespace generation.localgen {
    public class LocalMapGenerator {
        public LocalGenConfig localGenConfig = new(); // constants
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        public Dictionary<string, string> buildingsToGenerate = new();
        public Dictionary<string, string[]> itemsToStore = new();

        // generates local map data both to LocalMap and GameModel TODO: generate only to localmap
        public LocalModel generateLocalMap(string name, Vector2Int position) {
            localGenContainer = new LocalGenContainer(name);
            localGenContainer.buildingsToAdd = buildingsToGenerate;
            localGenContainer.itemsToStore = itemsToStore;
            localGenSequence = new LocalGenSequence(this);
            return localGenSequence.run();
        }
    }
}