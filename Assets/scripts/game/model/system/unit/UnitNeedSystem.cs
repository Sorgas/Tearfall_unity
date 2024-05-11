using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    // Rolls needs counters in NeedsComponent (hunger, thirst, rest) from 1 to 0.
    // Updates needs priorities
    public class UnitNeedSystem : LocalModelIntervalEcsSystem {
        private const int interval = GameTime.ticksPerMinute * 5; // every 5 in-game minutes
        private readonly float restTick;
        private readonly float hungerTick;
        private readonly float thirstTick;
        private EcsFilter<UnitNeedComponent> filter;

        public UnitNeedSystem() : base(interval) {
            restTick = RestNeed.perTickChange * interval;
            hungerTick = HungerNeed.perTickChange * interval;
            // thirstTick = ThirstNeed.perTickChange * interval;
        }
        
        protected override void runIntervalLogic(int updates) {
            foreach (var i in filter) {
                ref UnitNeedComponent component = ref filter.Get1(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                rollNeeds(unit, ref component, updates);
                // updateSlots(ref component, ref unit);
            }
        }

        // TODO add thirst
        // rolls needs values from 1 to 0
        private void rollNeeds(EcsEntity unit, ref UnitNeedComponent component, int updates) {
            bool recalculateEffects = false;
            component.hunger = clampValue(component.hunger, -hungerTick, updates);
            component.hungerPriority = Needs.hunger.getPriority(component.hunger);
            ref UnitStatusEffectsComponent effectsComponent = ref unit.takeRef<UnitStatusEffectsComponent>();
            string hungerEffect = Needs.hunger.getStatusEffect(component.hunger);
            if (effectsComponent.hungerNeedEffect != hungerEffect) {
                effectsComponent.hungerNeedEffect = hungerEffect;
                log("hunger effect set to " + hungerEffect);
                recalculateEffects = true;
            }
            component.rest = clampValue(component.rest, -restTick, updates);
            component.restPriority = Needs.rest.getPriority(component.rest);
            string restEffect = Needs.rest.getStatusEffect(component.rest);
            if (effectsComponent.restNeedEffect != restEffect) {
                effectsComponent.restNeedEffect = restEffect;
                log("rest effect set to " + restEffect);
                recalculateEffects = true;
            }
            if(recalculateEffects) model.unitContainer.statusEffectUtility.recalculate(unit);
        }

        private float clampValue(float value, float delta, int updates) {
            value += delta * updates;
            return value > 0 ? value : 0;
        }
        
        // private void updateSlots(ref UnitNeedComponent component, ref EcsEntity unit) {
        //     // todo add flag for updating
        //     if (unit.Has<UnitEquipmentComponent>()) {
        //         UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        //         component.slotsToFill = component.desiredSlots
        //             .Select(slot => equipment.slots[slot])
        //             .Where(slot => slot.item == EcsEntity.Null)
        //             .Select(slot => slot.name).ToList();
        //     }
        // }
    }
}