using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;

namespace game.model.component.task.action.combat {
// Continuous action for performing in combat. Selects nearest target, then creates action to hit it.
public class CombatAction : Action {
    private EcsEntity currentTarget; // 
    
    public CombatAction() : base(new SelfActionTarget()) {
        startCondition = () => {
            if (currentTarget == EcsEntity.Null) {
                currentTarget = selectTarget();
            }
            if (currentTarget == EcsEntity.Null) {
                return ActionCheckingEnum.OK; // will complete action successfully
            }
            if (currentTarget.Has<UnitComponent>()) {
                return addPreAction(new AttackAction(new MeleeUnitActionTarget(currentTarget)));
            }
            return ActionCheckingEnum.FAIL;
        };
        
    }

    private EcsEntity selectTarget() {
        return EcsEntity.Null;
    }
}
}