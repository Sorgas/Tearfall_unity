using enums.item.type;
using game.model;
using game.model.component;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.item {
    // creates sprite GO for items on ground. updates GO position for moved items
    public class ItemVisualSystem : IEcsRunSystem {
        // TODO add marker component instead of position
        public EcsFilter<ItemComponent, PositionComponent>.Exclude<ItemVisualComponent>
            newItemsFilter; // items put on ground but not rendered

        public EcsFilter<PositionComponent, ItemVisualComponent> itemsOnGroundFilter; // items on ground should update GO position
        private Vector3 spriteZOffset = new(0, 0, WALL_LAYER * GRID_STEP + GRID_STEP / 2);
        private Vector3 spriteZOffsetForRamp = new(0, 0, WALL_LAYER * GRID_STEP - GRID_STEP / 2);

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
            visual.go = PrefabLoader.create("Item", GameView.get().sceneObjectsContainer.mapHolder);
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = ItemTypeMap.get().getSprite(item.type);
            entity.Replace(visual);
        }

        private void updatePosition(ref ItemVisualComponent component, PositionComponent positionComponent) {
            Vector3Int pos = positionComponent.position;
            Vector3 scenePos = ViewUtil.fromModelToScene(pos) +
                               (GameModel.get().currentLocalModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE
                                   ? spriteZOffsetForRamp
                                   : spriteZOffset);
            component.spriteRenderer.gameObject.transform.localPosition = scenePos;
            component.spriteRenderer.sortingOrder = pos.z;
        }
    }
}