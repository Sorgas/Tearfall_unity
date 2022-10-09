using enums.plant;
using game.model.component;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.plant {
    public class PlantGenerator {

        // TODO add multi-block tree generator
        public EcsEntity generate(string typeName, EcsEntity entity) {
            PlantType type = PlantTypeMap.get().get(typeName);
            entity.Replace(new PlantComponent { block = createPlantBlock(entity, type), type = type });
            entity.Replace(new NameComponent { name = type.title });
            return entity;
        }

        public EcsEntity generate(string typeName, Vector3Int position, EcsEntity entity) {
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