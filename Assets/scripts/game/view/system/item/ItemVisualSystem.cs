using enums.item.type;
using game.model.component;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.item {

    public class ItemVisualSystem : IEcsRunSystem {
        public EcsFilter<ItemComponent, PositionComponent>.Exclude<ItemVisualComponent> newItemsFilter; // items put on ground but not rendered
        public EcsFilter<PositionComponent, ItemVisualComponent> itemsOnGroundFilter; // items on ground should update GO position
        private int SIZE = 32;
        private Vector2 pivot = new Vector2(0, 0);
        private GameObject itemPrefab = Resources.Load<GameObject>("prefabs/Item"); 

        public void Run() {
            foreach (var i in newItemsFilter) {
                EcsEntity entity = newItemsFilter.GetEntity(i);
                createSpriteForItem(entity);
            }
            foreach (var i in itemsOnGroundFilter) {
                updatePosition(ref itemsOnGroundFilter.Get2(i), itemsOnGroundFilter.Get1(i));
            }
        }

        private void createSpriteForItem(EcsEntity entity) {
            ItemComponent item = entity.Get<ItemComponent>();
            PositionComponent position = entity.Get<PositionComponent>();
            ItemVisualComponent visual = new ItemVisualComponent();
            visual.go = Object.Instantiate(itemPrefab, position.position, Quaternion.identity);
            visual.go.transform.SetParent(GameView.get().mapHolder);
            Vector3 spritePosition = ViewUtil.fromModelToScene(entity.Get<PositionComponent>().position);
            spritePosition.z -= 0.1f;
            visual.go.transform.localPosition = spritePosition;
            visual.spriteRenderer = visual.go.GetComponent<SpriteRenderer>();
            visual.spriteRenderer.sprite = createSprite(ItemTypeMap.getItemType(item.type));
            entity.Replace(visual);
        }

        private void updatePosition(ref ItemVisualComponent component, PositionComponent positionComponent) {
            Vector3Int pos = positionComponent.position;
            Vector3 scenePos = ViewUtil.fromModelToScene(pos);
            component.spriteRenderer.gameObject.transform.localPosition = scenePos;
        }
        
        private Sprite createSprite(ItemType type) {
            Sprite sprite = Resources.Load<Sprite>("tilesets/items/" + type.atlasName); // TODO move to ItemTypeMap
            Texture2D texture = sprite.texture;
            Rect rect = new Rect(type.atlasXY[0] * SIZE, texture.height - (type.atlasXY[1] + 1) * SIZE, SIZE, SIZE);
            return Sprite.Create(texture, rect, pivot, 32);
        }
    }
}