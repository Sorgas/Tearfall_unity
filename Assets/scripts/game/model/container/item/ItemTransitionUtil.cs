using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    
    // all item transitions in item container should be made through this class
    // unit's actions are mandatory to use this.
    public class ItemTransitionUtil : ItemContainerPart {

        public ItemTransitionUtil(ItemContainer container) : base(container) { }
        
        public void fromUnitToGround(EcsEntity item, EcsEntity unit, Vector3Int position) {
            container.equipped.removeItemFromUnit(item, unit);
            container.onMap.putItemToMap(item, position);
        }

        public void fromUnitToContainer(EcsEntity item, EcsEntity unit, EcsEntity container) {
            this.container.equipped.removeItemFromUnit(item, unit);
            this.container.stored.addItemToContainer(item, container);
        }

        public void fromGroundToUnit(EcsEntity item, EcsEntity unit) {
            container.onMap.takeItemFromMap(item);
            container.equipped.addItemToUnit(item, unit);
        }

        public void fromContainerToGround(EcsEntity item, EcsEntity container, Vector3Int position) {
            this.container.stored.removeItemFromContainer(item);
            this.container.onMap.putItemToMap(item, position);
        }

        public void fromContainerToUnit(EcsEntity item, EcsEntity container, EcsEntity unit) {
            this.container.stored.removeItemFromContainer(item);
            this.container.equipped.addItemToUnit(item, unit);
        }

        public void fromUnitToGround(EcsEntity item, EcsEntity unit) => fromUnitToGround(item, unit, unit.pos());
    }
}