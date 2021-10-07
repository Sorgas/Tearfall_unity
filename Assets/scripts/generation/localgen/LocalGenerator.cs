using util.geometry;

namespace generation.localgen {
    public abstract class LocalGenerator {
        protected LocalGenContainer container;
        protected LocalGenConfig config;
        protected IntBounds2 bounds = new IntBounds2();
        
        protected LocalGenerator() { 
            container = GenerationState.get().localGenContainer;
            config = GenerationState.get().localGenConfig;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
        }

        public abstract void generate();

        public abstract string getMessage();
    }
}
