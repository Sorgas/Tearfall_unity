using System.Collections.Generic;
using enums.action;
using enums.unit;
using enums.unit.need;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    public class UnitNeedComponentGenerator {
        public void generate(ref EcsEntity entity, CreatureType type) {
            UnitNeedComponent component = new UnitNeedComponent();
            component.needsToFullfill = new Dictionary<Need, TaskPriorityEnum>();
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
