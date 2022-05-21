using enums;
using enums.item.type;
using game.model;
using game.model.component;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.item {
    // creates sprite GO for items on ground. updates GO position for moved items
    public class ItemVisualSystem : IEcsRunSystem {
        public EcsFilter<ItemComponent, PositionComponent>.Exclude<ItemVisualComponent> newItemsFilter; // items put on ground but not rendered
        public EcsFilter<PositionComponent, ItemVisualComponent> itemsOnGroundFilter; // items on ground should update GO position
        private Vector3 spriteZOffset = new(0, 0, WALL_LAYER * GRID_STEP + GRID_STEP / 2);
        private Vector3 spriteZOffsetForRamp = new(0, 0, WALL_LAYER * GRID_STEP - GRID_STEP / 2);

        private const int SIZE = 32;
        private Vector2 pivot = new(0, 0);
        private GameObject itemPrefab = Resources.Load<GameObject>("prefabs/Item");
        private RectTransform mapHolder = GameView.get().sceneObjectsContainer.mapHolder;

        public void Run() {
            foreach (var i in newItemsFilter) {
                EcsEntity entity = newItemsFilter.GetEntity(i);
                createSpriteForItem(entity);
            }
            foreach (var i in itemsOnGroundFilter) {
                // todo add lastPosition to visual component
                updatePosition(ref itemsOnGroundFilter.Get2(i), itemsOnGroundFilter.Get1(i));
            }
        }

        private void createSpriteForItem(EcsEntity entity) {
            ItemComponent item = entity.takeRef<ItemComponent>();
            ItemVisualComponent visual = new();
            visual.go = Object.Instantiate(itemPrefab, mapHolder, true);
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = createSprite(ItemTypeMap.getItemType(item.type));
            entity.Replace(visual);
        }

        private void updatePosition(ref ItemVisualComponent component, PositionComponent positionComponent) {
            Vector3Int pos = positionComponent.position;
            Vector3 scenePos = ViewUtil.fromModelToScene(pos) + spriteZOffset;
            if (GameModel.localMap.blockType.get(pos) == BlockTypeEnum.RAMP.CODE) {
                scenePos.z -= GRID_STEP;
            }
            component.spriteRenderer.gameObject.transform.localPosition = scenePos;
            component.spriteRenderer.sortingOrder = pos.z;
        }

        private Sprite createSprite(ItemType type) {
            Sprite sprite = Resources.Load<Sprite>("tilesets/items/" + type.atlasName); // TODO move to ItemTypeMap
            Texture2D texture = sprite.texture;
            int x = type.atlasXY[0];
            int y = type.atlasXY[1];
            Rect rect = new(x * SIZE, texture.height - (y + 1) * SIZE, SIZE, SIZE);
            return Sprite.Create(texture, rect, pivot, 32);
        }
    }
}