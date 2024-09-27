using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// Continuous action for performing in combat. Selects nearest target of hostile faction, then creates action to hit it.
public class CombatAction : Action {
    private EcsEntity currentTarget;
    
    
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
        string performerFaction = performer.take<FactionComponent>().name;
        // TODO replace with hostile faction selection
        if (performerFaction == "player") {
            // TODO replace with nearest unit selection
            EcsEntity target = model.factionContainer.units.get("raider")[0];
            return target;
        } else if (performerFaction == "raider") {
            EcsEntity target = model.factionContainer.units.get("player")[0];
            return target;
        }
        return EcsEntity.Null;
    }
}
}