using System.Collections.Generic;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using MoreLinq;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// Continuous action for performing in combat. Selects nearest target of hostile faction, then creates action to hit it.
public class AggressiveCombatAction : Action {
    private EcsEntity currentTarget;

    public AggressiveCombatAction() : base(new SelfActionTarget()) {
        name = "aggressive combat action";
        startCondition = () => {
            if (currentTarget == EcsEntity.Null) {
                currentTarget = selectTarget();
            }
            if (currentTarget == EcsEntity.Null) {
                return ActionCheckingEnum.OK; // will complete action successfully
            }
            if (currentTarget.Has<UnitComponent>()) { // target not dead TODO add lethal flag, add downed property
                return addPreAction(new AttackUnitInMeleeAction(currentTarget));
            }
            return ActionCheckingEnum.FAIL;
        };
    }

    private EcsEntity selectTarget() {
        List<EcsEntity> units = model.factionContainer.getUnitsOfHostileFactions(performer);
        if (units.Count == 0) return EcsEntity.Null;
        Vector3Int performerPositon = performer.pos();
        return units.MinBy(unit => (unit.pos() - performerPositon).sqrMagnitude);
    }
}
}