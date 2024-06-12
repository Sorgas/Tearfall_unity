using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;

namespace game.model.system.unit {
// updates unit's status effects when need values reach some threshold
public class UnitNeedEffectsSystem : LocalModelIntervalEcsSystem {
    private const int interval = GameTime.ticksPerMinute * 5;
    public EcsFilter<UnitNeedComponent, UnitStatusEffectsComponent> filter;

    public UnitNeedEffectsSystem() : base(interval) { }

    protected override void runLogic(int updates) {
        foreach (var i in filter) {
            updateStatuses(filter.Get1(i), filter.Get2(i));
        }
    }

    private void updateStatuses(UnitNeedComponent needs, UnitStatusEffectsComponent effects) {
        string restEffect = Needs.rest.getStatusEffect(needs.rest);
        if (effects.restNeedEffect != restEffect) {
            
        }
        
    }
}
}