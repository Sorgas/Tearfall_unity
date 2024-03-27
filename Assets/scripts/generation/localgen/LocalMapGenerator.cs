using System.Collections.Generic;
using game.model.localmap;

// generates LocalMap by world and location in that world. 
namespace generation.localgen {
    public class LocalMapGenerator {
        public LocalGenConfig localGenConfig = new(); // constants
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        public Dictionary<string, string> buildingsToGenerate = new();
        public Dictionary<string, string[]> itemsToStore = new();
        
        public LocalModel generateLocalMap(string name) {
            localGenContainer = new LocalGenContainer(name);
            localGenContainer.buildingsToAdd = buildingsToGenerate;
            localGenContainer.itemsToStore = itemsToStore;
            localGenSequence = new LocalGenSequence(this);
            return localGenSequence.run();
        }
    }
}