using game.model.component.task.action.target;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// Action for performing single attack on target.
public class AttackAction : Action {
    private int preparationTicks;

    public AttackAction(ActionTarget target) : base(target) {
        displayProgressBar = false;
        preparationTicks = 3; // TODO use weapon stat
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
            UnitGoHandler handler = performer.take<UnitVisualComponent>().handler;
            handler.setOrientation(UnitOrientationsUtil.byVector(target.pos - performer.pos()));
            handler.actionAnimator.Play("attack");
            handler.attackAnimator.Play("attackleft");
            handler.toggleProgressBar(false);
            performer.Replace(new UnitAttackCooldownComponent { ticks = 20 }); // TODO use weapon cooldown
        };
    }
}
}