using System.Collections.Generic;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;

namespace generation.unit {
    public class UnitNeedComponentGenerator {
        public void generate(ref EcsEntity entity, CreatureType type) {
            UnitNeedComponent component = new UnitNeedComponent{rest = 0.8f, hunger = 0.8f, thirst = 0.8f};
            entity.Replace(component);
            if (type.desiredSlots.Count > 0) {
                UnitWearNeedComponent wear = new UnitWearNeedComponent();
                wear.desiredSlots = new List<string>(type.desiredSlots);
                entity.Replace(wear);
                Debug.Log("[UnitGenerator] UnitWearNeedComponent added to unit");
            }
        }
    }
}
