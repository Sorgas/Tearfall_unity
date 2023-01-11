using System.Collections.Generic;
using types.unit.body;
using UnityEngine;

namespace types.unit {
    public class CreatureTypeProcessor {
        // TODO
        // private static NeedEnum[] defaultNeeds = {NeedEnum.FOOD, NeedEnum.REST, NeedEnum.WATER}; // needs for most creatures
        private CreatureTypeMap typeMap;

        public CreatureTypeProcessor(CreatureTypeMap typeMap) {
            this.typeMap = typeMap;
        }

        public CreatureType process(RawCreatureType raw) {
            CreatureType type = new CreatureType(raw);
            // Debug.Log("processing creature type  " + type.name);

            if (!typeMap.templates.ContainsKey(raw.bodyTemplate)) {
                Debug.LogWarning("Creature " + type.name + " has invalid body template " + raw.bodyTemplate);
                return null;
            }

            // if(type.combinedAppearance != null) type.combinedAppearance.process();
            // Arrays.stream(GameplayStatEnum.values())
            //     .forEach(value -> type.statMap.put(value, value.DEFAULT)); // save default values
            // for (String statName : raw.statMap.keySet()) { // override default values
            //     if (GameplayStatEnum.map.containsKey(statName)) {
            //         type.statMap.put(GameplayStatEnum.map.get(statName), raw.statMap.get(statName));
            //     } else {
            //         Logger.LOADING.logError("Invalid stat name " + statName + " in creature type " + raw.name);
            //     }
            // }
            BodyTemplate template = typeMap.templates[raw.bodyTemplate];
            foreach (var part in template.body.Values) {
                type.bodyParts.Add(part.name, part.clone());
            }
            foreach (var slot in template.slots) {
                type.slots.Add(slot.Key, new List<string>(slot.Value));
            }
            type.desiredSlots.AddRange(template.desiredSlots);
            if (raw.desiredSlots.Count > 0) {
                type.desiredSlots.AddRange(raw.desiredSlots);
            }
            // type.needs.AddRange(defaultNeeds);
            return type;
        }
    }
}