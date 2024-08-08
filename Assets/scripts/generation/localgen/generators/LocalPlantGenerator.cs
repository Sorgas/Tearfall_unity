using System;
using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using generation.plant;
using Leopotam.Ecs;
using types;
using types.plant;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalPlantGenerator : LocalGenerator {
        private int maxAttempts = 20;
        private LocalMap map;
        private PlantGenerator generator;

        public LocalPlantGenerator(LocalMapGenerator generator) : base(generator) { }

        protected override void generateInternal() {
            map = container.map;
            generator = new(random(0, 1000));
            int plantsNumber = config.areaSize * config.areaSize / 100 * config.forestationLevel;
            plantsNumber += random(-1, 1) * 20;
            Dictionary<PlantType, float> plantTypes = getPlantTypes();
            foreach (PlantType type in plantTypes.Keys) {
                int number = (int)Math.Round(plantsNumber * plantTypes[type]);
                for (int i = 0; i < number; i++) {
                    Vector3Int position = findPlaceForPlant(type);
                    if(position.x >= 0) createPlant(position, type);
                }
            }
        }
        
        // TODO assess plant placing requirements
        private Vector3Int findPlaceForPlant(PlantType plantType) {
            for (int i = 0; i < maxAttempts; i++) {
                int x = random(0, map.bounds.maxX);
                int y = random(0, map.bounds.maxY);
                int z = findZ(x, y);
                Vector3Int position = new(x, y, z);
                if (z >= 0 && checkPlantOverlap(position)) return position;
            }
            return Vector3Int.left;
        }

        private int findZ(int x, int y) {
            for (int z = map.bounds.maxZ; z > 0; z--) {
                if (map.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE) {
                    return z;
                }
            }
            return -1;
        }

        // TODO check blocks of multi-tile trees
        private bool checkPlantOverlap(Vector3Int position) {
            return container.model.plantContainer.getBlock(position) == null;
        }
        
        private void createPlant(Vector3Int position, PlantType type) {
            float age = random(0, type.maxAge);
            EcsEntity entity = generator.generate(type.name, age, container.model.createEntity());
            container.model.plantContainer.addPlant(entity, position);
        }

        // TODO make weighted plant list by local biome
        private Dictionary<PlantType, float> getPlantTypes() {
            return (new[] {"bush", "raspberryBush"})
                .Select(typeName => PlantTypeMap.get().get(typeName))
                .ToDictionary(type => type, _ => 0.5f);
        }
        
        public override string getMessage() {
            return "generating plants...";
        }
    }
}