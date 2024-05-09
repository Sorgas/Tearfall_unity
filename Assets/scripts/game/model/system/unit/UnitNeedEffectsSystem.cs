using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;

namespace game.model.system.unit {
// updates unit's status effects when need values reach some threshold
public class UnitNeedEffectsSystem : LocalModelIntervalEcsSystem {
    private const int interval = GameTime.ticksPerMinute * 5;
    public EcsFilter<UnitNeedComponent, UnitHealthComponent> filter;

    public UnitNeedEffectsSystem() : base(interval) { }

    protected override void runIntervalLogic(int updates) {
        foreach (var i in filter) {
            filter.Get1(i);
        }
    }

    private void updateStatuses(UnitNeedComponent needs, UnitHealthComponent health) {
        string restEffect = Needs.rest.getStatusEffect(needs.rest);
               
    }
}
}