using UnityEngine;
using util.geometry.bounds;
using Random = System.Random;

namespace generation.localgen {
    public abstract class LocalGenerator {
        protected LocalMapGenerator localMapGenerator;
        protected LocalGenContainer container;
        protected LocalGenConfig config;
        protected IntBounds2 bounds = new();
        protected string name = "LocalGenerator";
        protected bool debug = false;
        private Random numberGenerator;
        
        protected LocalGenerator(LocalMapGenerator generator) {
            localMapGenerator = generator; 
            container = generator.localGenContainer;
            config = generator.localGenConfig;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
        }

        public void generate() {
            numberGenerator = localMapGenerator.localGenSequence.random;
            generateInternal();
        }

        protected abstract void generateInternal();

        public abstract string getMessage();

        protected int random(int min, int max) => numberGenerator.Next(min, max);

        protected float random(float min, float max) => (min + random() * (max - min));

        protected float random() => (float)numberGenerator.NextDouble();
        
        protected void log(string message) {
            if(debug) Debug.Log($"[{name}]: {message}");
        }
    }
}
