using System.Collections.Generic;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
    public class ItemActionTarget : EntityActionTarget {
        public EcsEntity item;

        // todo add contained items
        public override Vector3Int pos => item.pos();

        public ItemActionTarget(EcsEntity item) : base(item, EXACT) {
            this.item = item;
        }

        public override List<Vector3Int> getPositions(LocalModel model) {
            throw new System.NotImplementedException();
        }
        
        public new ActionTargetStatusEnum check(EcsEntity performer, LocalModel model) {
            if (performer.takeRef<UnitEquipmentComponent>().hauledItem == entity) {
                return ActionTargetStatusEnum.READY; // item is in hands already
            }
            return base.check(performer, model);
        }
    }
}