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
            action.perform(ticks);
            if (action.status == ActionStatusEnum.COMPLETE) {
                Debug.Log("[UnitActionPerformingSystem]: action " + action.name + " complete");
                unit.Del<UnitCurrentActionComponent>();
            }
        }
    }
}
}