using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.system.unit {
// runs active actions for units with current action.
// Units can get currentActionComponent only after passing all action checks.
public class UnitActionPerformingSystem : LocalModelScalableEcsSystem {
    public EcsFilter<UnitCurrentActionComponent> filter;

    protected override void runLogic(int ticks) {
        foreach (int i in filter) {
            EcsEntity unit = filter.GetEntity(i);
            Action action = filter.Get1(i).action;
            if (action.status == ActionStatusEnum.OPEN) { // first execution of perform()
                action.status = ActionStatusEnum.ACTIVE;
                action.onStart.Invoke();
            }
            action.ticksConsumer.Invoke(ticks);
            if (action.finishCondition.Invoke()) { // last execution of perform()
                action.onFinish.Invoke();
                action.status = ActionStatusEnum.COMPLETE;
            }
            if (action.status == ActionStatusEnum.COMPLETE) {
                Debug.Log("[UnitActionPerformingSystem]: action " + action.name + " complete");
                unit.Del<UnitCurrentActionComponent>();
                unit.Del<UnitCurrentAnimatedActionComponent>();
            }
        }
    }
}

public enum ActionStatusEnum {
    OPEN, // action not started
    ACTIVE,
    COMPLETE
}
}