using game.model.component.unit;
using Leopotam.Ecs;

namespace game.model.system.unit {
// Weapons equipped on units have cooldowns on attacks.
// This system rolls cooldowns even when attack action is not being performed.
public class UnitEquipmentSlotCooldownSystem : LocalModelScalableEcsSystem {
    private EcsFilter<UnitEquipmentComponent> filter;
    
    protected override void runLogic(int ticks) {
        foreach (var i in filter) {
            ref UnitEquipmentComponent component = ref filter.Get1(i);
            foreach (var slot in component.grabSlots.Values) {
                if (slot.cooldown > 0) slot.cooldown -= ticks;
            }
        }
    }
}
}