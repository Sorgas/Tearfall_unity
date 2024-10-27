using System.Linq;
using game.model.component.building;
using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using util;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// Continuous action for attacking single target until it's downed.
// Assesses distance to target and performers weapons, 
public class EngageAction : Action {
    private EcsEntity currentTarget;
    
    public EngageAction(EcsEntity target) : base(null) {
        if (target.Has<UnitComponent>()) {
            this.target = new MeleeUnitActionTarget(target);
        } else if (target.Has<BuildingComponent>()) {
            this.target = new BuildingActionTarget(target, ActionTargetTypeEnum.NEAR);
        } else {
            throw new GameException("Non-unit or building entity set as attack target");
        }
        currentTarget = target;
        startCondition = () => {
            if (!targetIsDefeated()) {
                Action action = selectAction();
                if (action != null) {
                    return addPreAction(action);
                }
            }
            return ActionCheckingEnum.OK; // will complete action successfully
        };
    }

    private bool targetIsDefeated() {
        return currentTarget.Has<UnitComponent>() && !currentTarget.Has<UnitDownedComponent>();
    }

    private Action selectAction() {
        UnitEquipmentComponent equipment = performer.take<UnitEquipmentComponent>();
        EcsEntity rangedWeapon = equipment.getRangedWeapon();
        if (rangedWeapon != EcsEntity.Null) {
            int distanceToTarget = (currentTarget.pos() - performer.pos()).sqrMagnitude;
            // compare weapon and target distance, 
            return null; // TODO
        } else {
            return new AttackUnitInMeleeAction(currentTarget);
        }
    }
}
}