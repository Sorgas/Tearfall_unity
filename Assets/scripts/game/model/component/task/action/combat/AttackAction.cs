using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.combat {
// Action for performing single attack on target.
public class AttackAction : Action {
    private int preparationTicks;

    public AttackAction(ActionTarget target) : base(target) {
        preparationTicks = 3;
        startCondition = () => ActionCheckingEnum.OK;
        maxProgress = 1f;
        ticksConsumer = ticks => {
            if (!performer.Has<UnitAttackCooldownComponent>()) {
                preparationTicks -= ticks;
            }
        };
        finishCondition = () => preparationTicks <= 0;
        onFinish = () => {
            Debug.Log("hit performed");
            performer.Replace(new UnitAttackCooldownComponent { ticks = 20 }); // TODO use weapon cooldown
        };
    }
}
}