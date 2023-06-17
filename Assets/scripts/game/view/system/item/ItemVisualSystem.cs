using game.model;
using game.model.component;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.Rendering;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.item {
    // creates sprite GO for items on ground. updates GO position for moved items
    // TODO add item sprite GO monobeh handler
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
                EcsEntity entity = itemsOnGroundFilter.GetEntity(i);
                ref ItemVisualComponent visual = ref itemsOnGroundFilter.Get2(i);
                updatePosition(ref visual, itemsOnGroundFilter.Get1(i));
                updateLockedIcon(entity, visual);
            }
        }

        private void createSpriteForItem(EcsEntity entity) {
            ItemComponent item = entity.takeRef<ItemComponent>();
            ItemVisualComponent visual = new();
            Sprite sprite = ItemTypeMap.get().getSprite(item.type);
            GameObject go = PrefabLoader.create("Item", GameView.get().sceneObjectsContainer.mapHolder);
            go.name = "Item " + item.type + " " + item.materialString;
            visual.go = go;
            visual.spriteRenderer = go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = sprite;
            visual.spriteRenderer.color = MaterialMap.get().material(item.material).color;
            visual.iconGo = go.transform.GetChild(0).gameObject;
            visual.iconRenderer = visual.iconGo.GetComponent<SpriteRenderer>();
            visual.sortingGroup = go.GetComponent<SortingGroup>();
            visual.sprite = sprite;
            entity.Replace(visual);
        }

        private void updatePosition(ref ItemVisualComponent component, PositionComponent positionComponent) {
            Vector3Int pos = positionComponent.position;
            Vector3 scenePos = ViewUtil.fromModelToSceneForUnit(pos, GameModel.get().currentLocalModel);
            // Vector3 scenePos = ViewUtil.fromModelToScene(pos) +
            //                    (GameModel.get().currentLocalModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE
            //                        ? spriteZOffsetForRamp
            //                        : spriteZOffset);
            component.spriteRenderer.gameObject.transform.localPosition = scenePos;
            if (GlobalSettings.useSpriteSortingLayers) {
                component.sortingGroup.sortingOrder = pos.z;
            }
        }

        private void updateLockedIcon(EcsEntity entity, ItemVisualComponent component) {
            if (entity.Has<LockedComponent>() != component.iconGo.activeSelf) {
                component.iconGo.SetActive(entity.Has<LockedComponent>());
            }
        }
    }
}