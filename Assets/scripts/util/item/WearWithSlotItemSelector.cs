using System.Collections.Generic;
using game.model.component.item;
using Leopotam.Ecs;
using util.lang.extension;

namespace util.item {
    public class WearWithSlotItemSelector : ItemSelector {
        public List<string> slots;

        public WearWithSlotItemSelector(List<string> slots) {
            this.slots = slots;
        }

        public override bool checkItem(EcsEntity item) {
            if (item.Has<ItemWearComponent>()) {
                return slots.Contains(item.takeRef<ItemWearComponent>().slot);
            }
            return false;
        }
    }
}