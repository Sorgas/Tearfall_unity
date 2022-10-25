using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
    public class ItemActionTarget : EntityActionTarget {
        public EcsEntity item;

        public override Vector3Int? Pos =>
            // todo add contained items
            item.pos();

        public ItemActionTarget(EcsEntity item) : base(item, EXACT) {
            this.item = item;
        }
        
        public new ActionTargetStatusEnum check(EcsEntity performer, LocalModel model) {
            if (performer.takeRef<UnitEquipmentComponent>().hauledItem == entity) {
                return ActionTargetStatusEnum.READY; // item is in hands already
            }
            return base.check(performer, model);
        }
    }
}