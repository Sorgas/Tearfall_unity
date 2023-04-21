using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;
using static game.model.component.plant.PlantUpdateType;

namespace generation.plant {
    public class PlantGenerator {

        // TODO add multi-block tree generator
        public EcsEntity generate(string typeName, EcsEntity entity) {
            PlantType type = PlantTypeMap.get().get(typeName);
            entity.Replace(new PlantComponent { block = createPlantBlock(entity, type), type = type });
            entity.Replace(new NameComponent { name = type.title });
            entity.Replace(new PlantVisualUpdateComponent{type = NEW});
            
            return entity;
        }

        public EcsEntity generateFromSeed(EcsEntity seedItem, Vector3Int position, EcsEntity entity) {
            string plantName = seedItem.take<ItemSeedComponent>().plant;
            // TODO add age
            return generate(plantName, position, entity);
        }
        
        private EcsEntity generate(string typeName, Vector3Int position, EcsEntity entity) {
            generate(typeName, entity);
            entity.Replace(new PositionComponent { position = position });
            return entity;
        }

        private PlantBlock createPlantBlock(EcsEntity entity, PlantType type) {
            PlantBlock block = new(type.material, PlantBlockTypeEnum.TRUNK.code);
            block.plant = entity;
            return block;
        }
    }
}