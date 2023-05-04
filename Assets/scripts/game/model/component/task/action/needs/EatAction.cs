using game.model.component.item;
using game.model.component.task.action.equipment;
using game.model.component.task.action.equipment.obtain;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using types.unit.need;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.needs {
    public class EatAction : ItemAction {
        public Vector3Int tableBlock;
        public EcsEntity chair;
        public bool started = false; // used in unit drawer

        public EatAction(EcsEntity item) : base(new SelfActionTarget(), item) {
            name = "eat action";
            startCondition = () => {
                if (!item.Has<ItemFoodComponent>()) return FAIL; // item is not food

                if (!equipment().items.Contains(item) && equipment().hauledItem != item) {
                    log("food item not in equipment");
                    return addPreAction(new ObtainItemAction(item)); // find item
                }

                // units will eat without table on highest priority
                if (performer.take<UnitNeedComponent>().hungerPriority != TaskPriorityEnum.SAFETY) {
                    EcsEntity chair = findChairWithTable();
                    if (chair != EcsEntity.Null) {
                        lockEntity(chair);
                        if (performer.pos() != chair.pos()) return addPreAction(new MoveAction(chair.pos()));
                    }
                }

                return OK;
            };

            // TODO orient unit, put item to table
            onStart = () => {
                //            speed = 1f * task.performer.get(HealthAspect.class).properties.get("performance");
                //            maxProgress = 400;
            };

            onFinish = () => {
                EcsEntity performer = this.performer;
                ref UnitNeedComponent component = ref performer.takeRef<UnitNeedComponent>();
                log("eating " + item.take<ItemFoodComponent>().nutrition);
                component.hunger += item.take<ItemFoodComponent>().nutrition;
                component.hungerPriority = Needs.hunger.getPriority(component.hunger);
                log("eaten: " + component.hunger + " " + component.hungerPriority);
                model.itemContainer.equipped.removeItemFromUnit(item, performer);
                if(equipment().hauledItem == item) equipment().hauledItem = EcsEntity.Null;
                equipment().items.Remove(item);
                item.Destroy();
            };
        }

        private EcsEntity findChairWithTable() {
            return model.buildingContainer.util.findFreeChairWithTable(performer.pos());
        }
    }
}