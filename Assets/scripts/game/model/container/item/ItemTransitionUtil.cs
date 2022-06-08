using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    
    // all item transitions in item container should be made through this class
    public class ItemTransitionUtil : ItemContainerPart {

        public ItemTransitionUtil(ItemContainer container) : base(container) { }
        
        public void fromUnitToGround(EcsEntity item, EcsEntity unit, Vector3Int position) {
            container.equipped.removeItemFromUnit(item, unit);
            container.onMap.putItemToMap(item, position);
        }

        public void fromUnitToContainer(EcsEntity item, EcsEntity unit, EcsEntity container) {
            this.container.equipped.removeItemFromUnit(item, unit);
            // TODO register to container
        }

        public void fromGroundToUnit(EcsEntity item, EcsEntity unit) {
            container.onMap.takeItemFromMap(item);
            container.equipped.addItemToUnit(item, unit);
        }

        public void fromContainerToGround(EcsEntity item, EcsEntity container, Vector3Int position) {
            // TODO unregister from container
            this.container.onMap.putItemToMap(item, position);
            
        }

        public void fromContainerToUnit(EcsEntity item, EcsEntity container, EcsEntity unit) {
            // TODO unregister from container
            this.container.equipped.addItemToUnit(item, unit);
        }

        public void fromUnitToGround(EcsEntity item, EcsEntity unit) => fromUnitToGround(item, unit, unit.pos());
    }
}