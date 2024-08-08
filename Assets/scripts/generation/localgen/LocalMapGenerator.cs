using System.Collections.Generic;
using game.model.localmap;

// generates LocalMap by world and location in that world. 
namespace generation.localgen {
    public class LocalMapGenerator {
        public readonly LocalGenConfig localGenConfig = new(); // constants
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        // TODO move to preparation state
        public readonly Dictionary<string, string> testBuildingsToGenerate = new();
        public readonly Dictionary<string, string[]> testItemsToStore = new();
        
        public LocalModel generateLocalMap(string name) {
            localGenContainer = new LocalGenContainer(name);
            localGenContainer.buildingsToAdd = testBuildingsToGenerate;
            localGenContainer.itemsToStore = testItemsToStore;
            localGenSequence = new LocalGenSequence(this);
            return localGenSequence.run();
        }
    }
}