using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.component.task.action.equipment.use {
    public class EquipWearItemAction : PutItemToDestinationAction {
        public EquipWearItemAction(EcsEntity targetItem) : base(new SelfActionTarget(), targetItem) {
            onFinish = () => {
                ItemWearComponent wear = targetItem.takeRef<ItemWearComponent>();
                EquipmentSlot targetSlot = equipment().slots[wear.slot];

                // TODO wrap with layer condition

                targetSlot.item = targetItem;
                equipment().hauledItem = EcsEntity.Null;
                updateWearNeed();
            };
        }

        protected new bool validate() {
            ItemWearComponent wear = item.takeRef<ItemWearComponent>();
            return base.validate() && equipment().slots[wear.slot] != null;
        }

        private void updateWearNeed() {
            if (performer.Has<UnitWearNeedComponent>()) {
                performerRef.takeRef<UnitWearNeedComponent>().valid = false;
            }
        }
    }
}