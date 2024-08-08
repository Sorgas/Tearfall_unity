using game.model;
using game.model.localmap;
using generation;
using Leopotam.Ecs;
using types;
using UnityEngine;
using static types.BlockTypes;

namespace game {
    public class TestLevelInitializer2 {
        public void createTestLocalMap(string modelName) {
            if (GameModel.get().world != null) {
                Debug.LogWarning("world already exists in GameModel");
                return;
            }
            GenerationState state = GenerationState.get();
            state.worldGenConfig.size = 10;
            state.generateWorld(); // sets world map to game model
            Vector2Int position = new(5, 5);
            state.localMapGenerator.localGenConfig.location = position;
            
            LocalModel localModel = createModel();
            spawnSettlers(localModel);
            GameModel.get().addLocalModel(modelName, localModel);
        }

        private LocalModel createModel() {
            LocalModel model = new();
            model.localMap = new LocalMap(50, 50, 20, model);
            for (int x = 0; x < 50; x++) {
                for (int y = 0; y < 50; y++) {
                    for (int z = 0; z < 5; z++) {
                        model.localMap.blockType.setRaw(x, y, z, WALL.CODE, "soil");
                    }
                    model.localMap.blockType.setRaw(x, y, 5, FLOOR.CODE, "soil");
                }
            }
            changeBlocksAroundUnit(model.localMap, 5, FLOOR, FLOOR);
            changeBlocksAroundUnit(model.localMap, 6, FLOOR, RAMP);
            changeBlocksAroundUnit(model.localMap, 7, FLOOR, WALL);
            changeBlocksAroundUnit(model.localMap, 8, WALL, FLOOR);
            changeBlocksAroundUnit(model.localMap, 9, WALL, RAMP);
            changeBlocksAroundUnit(model.localMap, 10, WALL, WALL);
            changeBlocksAroundUnit(model.localMap, 11, FLOOR, FLOOR);
            changeBlocksAroundUnit(model.localMap, 12, FLOOR, RAMP);
            changeBlocksAroundUnit(model.localMap, 13, FLOOR, WALL);
            changeBlocksAroundUnit(model.localMap, 13, WALL, WALL);
            model.localMap.blockType.setRaw(14, 5, 5, RAMP.CODE, "soil");
            return model;
        }

        private void spawnSettlers(LocalModel model) {
            for (int i = 0; i < 1; i++) {
                Vector3Int spawnPoint = new(5 + i, 5, 5);
                EcsEntity entity = model.createEntity();
                // UnitGenerator unitGenerator = new();
                // unitGenerator.generateUnit(new SettlerData {name = "settler", age = 30, type = "human"}, entity);
                // ref PositionComponent positionComponent = ref entity.Get<PositionComponent>();
                // positionComponent.position = spawnPoint;
                //
                // UnitJobsComponent jobsComponent = entity.Get<UnitJobsComponent>();
                // for (var j = 0; j < Jobs.all.Length; j++) {
                //     jobsComponent.enabledJobs.Add(Jobs.all[j].name, 1);
                // }
            }
        }

        private void changeBlocksAroundUnit(LocalMap map, int x, BlockType front, BlockType back) {
            map.blockType.setRaw(x, 4, 5, front.CODE, "soil");
            if(front == WALL) map.blockType.setRaw(x, 4, 6, FLOOR.CODE, "soil");
            map.blockType.setRaw(x, 6, 5, back.CODE, "soil");
            if(back == WALL) map.blockType.setRaw(x, 6, 6, FLOOR.CODE, "soil");
        }
    }
}