using game.model.component.plant;
using game.view.util;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;
using static game.model.component.plant.PlantUpdateType;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.plant {
    public class PlantVisualUpdateSystem : IEcsRunSystem {
        public EcsFilter<PlantVisualUpdateComponent> filter;
        private readonly Vector3 zOffset = new(0, 0, WALL_LAYER * GRID_STEP + GRID_STEP / 2f);
        private bool debug = false;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                PlantVisualUpdateComponent update = filter.Get1(i);
                entity.Del<PlantVisualUpdateComponent>();
                PlantComponent plant = entity.take<PlantComponent>();
                PlantType plantType = plant.type;
                int growthStage = getPlantStage(entity);
                foreach (PlantUpdateType type in update.updates) {
                    switch (type) {
                        case STAGE_CHANGE:
                            ref PlantVisualComponent visual = ref entity.takeRef<PlantVisualComponent>();
                            visual.spriteRenderer.sprite = PlantTypeMap.get().spriteMap.getSprite(plantType.name, growthStage);
                            visual.tileNumber = growthStage;
                            log("plant sprite changed to next stage");
                            break;
                        case NEW:
                            entity.Replace(createVisualComponent(entity, plant, growthStage));
                            log("plant sprite created");
                            break;
                        case REMOVE:
                            Object.Destroy(entity.Get<PlantVisualComponent>().go);
                            entity.Destroy();
                            log("plant destroyed");
                            break;
                        case HARVEST_READY:
                            addHarvestSprite(entity);
                            log("plant harvest sprite added");
                            break;
                    }
                }
            }
        }

        private int getPlantStage(EcsEntity entity) {
            if (entity.Has<PlantGrowthComponent>()) {
                return entity.take<PlantGrowthComponent>().currentStage;
            }
            return entity.take<PlantComponent>().type.growthStages.Length - 1;
        }

        private PlantVisualComponent createVisualComponent(EcsEntity entity, PlantComponent plant, int stage) {
            PlantVisualComponent visual = new();
            Vector3 spritePosition = ViewUtil.fromModelToScene(entity.pos()) + zOffset;
            visual.go = PrefabLoader.create("Plant", GameView.get().sceneObjectsContainer.mapHolder);
            visual.go.transform.localPosition = spritePosition;
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = PlantTypeMap.get().spriteMap.getSprite(plant.type.name, stage);
            visual.spriteRenderer.sortingOrder = entity.pos().z;
            return visual;
        }

        private void addHarvestSprite(EcsEntity entity) {

        }

        private void log(string message) {
            if (debug) Debug.Log("[PlantVisualUpdateSystem] " + message);
        }
    }
}