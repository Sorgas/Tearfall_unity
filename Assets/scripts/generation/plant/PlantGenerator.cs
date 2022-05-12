using enums;
using enums.plant;
using game.model;
using game.model.component;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.plant {
    public class PlantGenerator {

        public EcsEntity generate(string typeName) {
            PlantType type = PlantTypeMap.get().get(typeName);
            EcsEntity entity = GameModel.get().createEntity();
            entity.Replace(new PlantComponent { block = createPlantBlock(entity, type), type = type });
            return entity;
        }

        public EcsEntity generate(string typeName, Vector3Int position) {
            EcsEntity entity = generate(typeName);
            entity.Replace(new PositionComponent { position = position });
            return entity;
        }

        private PlantBlock createPlantBlock(EcsEntity entity, PlantType type) {
            PlantBlock block = new(type.material, BlockTypeEnum.WALL.CODE);
            block.plant = entity;
            return block;
        }
    }
}