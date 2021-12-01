using game.model.component;
using game.model.component.item;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.item {

    public class ItemVisualSystem : IEcsRunSystem {
        public EcsFilter<ItemComponent>.Exclude<ItemVisualComponent> newItemsFilter;
        public EcsFilter<PositionComponent, ItemVisualComponent> itemsOnGroundFilter;

        public void Run() {
            foreach (var i in newItemsFilter) {
                EcsEntity entity = newItemsFilter.GetEntity(i);
                entity.Replace(new ItemVisualComponent());
                createSpriteForItem(ref entity.Get<ItemVisualComponent>());
            }

            foreach (var i in itemsOnGroundFilter) {
                updatePosition(ref itemsOnGroundFilter.Get2(i), itemsOnGroundFilter.Get1(i));
            }
        }

        private void createSpriteForItem(ref ItemVisualComponent component) {
            GameObject prefab = Resources.Load<GameObject>("prefabs/Item");
            GameObject instance = Object.Instantiate(prefab, new Vector3(), Quaternion.identity);
            component.spriteRenderer = instance.GetComponent<SpriteRenderer>();
            instance.transform.SetParent(GameView.get().mapHolder);
        }

        private void updatePosition(ref ItemVisualComponent component, PositionComponent positionComponent) {
            Vector3Int pos = positionComponent.position;
            component.spriteRenderer.gameObject.transform.localPosition = new Vector3(pos.x, pos.y + pos.z / 2f + 0.25f, -pos.z * 2 - 0.1f);
        }
    }
}