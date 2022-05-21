using enums.plant;
using game.model.component.plant;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.plant {
    public class PlantVisualSystem : IEcsRunSystem {
        public EcsFilter<PlantComponent>.Exclude<PlantVisualComponent> filter;

        private GameObject plantPrefab = Resources.Load<GameObject>("prefabs/Plant");
        private RectTransform mapHolder = GameView.get().sceneObjectsContainer.mapHolder;
        private const int SIZE_X = 64;
        private const int SIZE_Y = 90;
        private readonly Vector2 pivot = new(0, 0);
        private readonly Vector3 zOffset = new(0, 0, WALL_LAYER * GRID_STEP + GRID_STEP / 2f);

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                PlantComponent plant = filter.Get1(i);
                entity.Replace(createVisualComponent(entity, plant));
                Debug.Log("plantSprite created");
            }
        }

        private PlantVisualComponent createVisualComponent(EcsEntity entity, PlantComponent plant) {
            PlantVisualComponent visual = new();
            Vector3 spritePosition = ViewUtil.fromModelToScene(entity.pos()) + zOffset;
            visual.go = Object.Instantiate(plantPrefab, spritePosition, Quaternion.identity);
            visual.go.transform.SetParent(mapHolder);
            visual.go.transform.localPosition = spritePosition;
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = createSprite(plant.type);
            visual.spriteRenderer.sortingOrder = entity.pos().z;
            return visual;
        }

        private Sprite createSprite(PlantType type) {
            Sprite sprite = Resources.Load<Sprite>("tilesets/plants/" + type.atlasName); // TODO move to ItemTypeMap & TileSetLoader
            Texture2D texture = sprite.texture;
            int x = type.tileXY[0];
            int y = type.tileXY[1];
            Rect rect = new(x * SIZE_X, texture.height - (y + 1) * SIZE_Y, SIZE_X, SIZE_Y);
            return Sprite.Create(texture, rect, pivot, 64);
        }
    }
}