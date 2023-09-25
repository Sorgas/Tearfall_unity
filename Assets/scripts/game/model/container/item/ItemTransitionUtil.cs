using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util;
using util.lang.extension;

namespace game.model.container.item {
    // all item transitions in item container should be made through this class
    // unit's actions are mandatory to use this.
    public class ItemTransitionUtil : ItemContainerPart {
        public ItemTransitionUtil(LocalModel model, ItemContainer container) : base(model, container) { }

        public void fromUnitToGround(EcsEntity item, EcsEntity unit, Vector3Int position) {
            container.equipped.removeItemFromUnit(item, unit);
            container.onMap.putItemToMap(item, position);
        }

        public void fromUnitToContainer(EcsEntity item, EcsEntity unit, EcsEntity containerEntity) {
            container.equipped.removeItemFromUnit(item, unit);
            container.stored.addItemToContainer(item, containerEntity);
        }

        public void fromGroundToUnit(EcsEntity item, EcsEntity unit) {
            container.onMap.takeItemFromMap(item);
            container.equipped.addItemToUnit(item, unit);
        }

        public void fromContainerToGround(EcsEntity item, EcsEntity containerEntity, Vector3Int position) {
            container.stored.removeItemFromContainer(item);
            container.onMap.putItemToMap(item, position);
        }

        public void fromContainerToUnit(EcsEntity item, EcsEntity containerEntity, EcsEntity unit) {
            container.stored.removeItemFromContainer(item);
            container.equipped.addItemToUnit(item, unit);
        }

        // should be the only place where item entities are destroyed
        public void destroyItem(EcsEntity item) {
            if (item.hasPos()) {
                container.onMap.takeItemFromMap(item);
            } else if (item.Has<ItemContainedComponent>()) {
                container.stored.removeItemFromContainer(item);
            } else if (container.equipped.itemEquipped(item)) {
                container.equipped.removeItemFromUnit(item);
            } else {
                throw new GameException("invalid item placement detected");
            }
            item.Destroy();
        }
        
        public void fromUnitToGround(EcsEntity item, EcsEntity unit) => fromUnitToGround(item, unit, unit.pos());
    }
}