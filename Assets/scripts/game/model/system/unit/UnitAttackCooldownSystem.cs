using game.model.component.unit;
using Leopotam.Ecs;

namespace game.model.system.unit {
// rolls cooldown time for attacks
public class UnitAttackCooldownSystem : LocalModelScalableEcsSystem {
    private EcsFilter<UnitAttackCooldownComponent> filter;
    
    protected override void runLogic(int ticks) {
        foreach (var i in filter) {
            ref UnitAttackCooldownComponent component = ref filter.Get1(i);
            component.ticks -= ticks;
            if (component.ticks <= 0) {
                filter.GetEntity(i).Del<UnitAttackCooldownComponent>();
            }
        }
    }
}
}