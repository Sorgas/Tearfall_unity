using UnityEngine;
using util.geometry.bounds;

namespace generation.localgen {
    public abstract class LocalGenerator {
        protected LocalMapGenerator localMapGenerator;
        protected LocalGenContainer container;
        protected LocalGenConfig config;
        protected IntBounds2 bounds = new IntBounds2();
        protected bool debug = false;
        
        protected LocalGenerator(LocalMapGenerator generator) {
            this.localMapGenerator = generator; 
            container = generator.localGenContainer;
            config = generator.localGenConfig;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
        }

        public abstract void generate();

        public abstract string getMessage();

        protected void log(string message) {
            if(debug) Debug.Log(message);
        }
    }
}
