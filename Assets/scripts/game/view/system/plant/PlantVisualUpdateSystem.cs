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

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                var type = filter.Get1(i).type;
                PlantComponent plant = entity.take<PlantComponent>();
                if (type == GROW) {
                    ref PlantVisualComponent visual = ref entity.takeRef<PlantVisualComponent>();
                    PlantType plantType = plant.type;
                    visual.spriteRenderer.sprite = PlantTypeMap.get().spriteMap.getSprite(plantType.name, plant.currentStage);
                    visual.tileNumber = plant.currentStage;
                    continue;
                }
                if (type == NEW) {
                    entity.Replace(createVisualComponent(entity, plant));
                    // create visual component
                }
                filter.GetEntity(i).Del<PlantVisualUpdateComponent>();
            }
        }

        private PlantVisualComponent createVisualComponent(EcsEntity entity, PlantComponent plant) {
            PlantVisualComponent visual = new();
            Vector3 spritePosition = ViewUtil.fromModelToScene(entity.pos()) + zOffset;
            visual.go = PrefabLoader.create("Plant", GameView.get().sceneObjectsContainer.mapHolder);
            visual.go.transform.localPosition = spritePosition;
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = PlantTypeMap.get().spriteMap.getSprite(plant.type.name, plant.currentStage);
            visual.spriteRenderer.sortingOrder = entity.pos().z;
            return visual;
        }
    }
}