using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model.container.item {
    /**
    * System for updating items in creature equipment, and modifying equipment.
    * All methods are atomic. More complex manipulations with item should be implemented with {@link stonering.entity.job.action.Action}s.
    * Does not takes or puts items to map or containers, this should be done by actions.
    * 
    * Creature have slots for wear with different layers of wears, slots for tools and other items(grab slots).
    * To equip some wear item or do any other manipulation with equipment, unit should has at least one grab slot free.
    * Unit hauls items in grab slots.
    * TODO add item requirements (1/2 hands)
    *
    * @author Alexander on 06.02.2020.
    */
    public class EquippedItemsManager {
        public Dictionary<EcsEntity, EcsEntity> equippedItems = new(); // item -> unit
        
        // registers item as equipped by unit
        public void addItemToUnit(EcsEntity item, EcsEntity unit) {
            equippedItems.Add(item, unit);
        }
        
        // unregisters item as equipped by unit
        public void removeItemFromUnit(EcsEntity item, EcsEntity unit) {
            equippedItems.Remove(item);
        }
    }
}