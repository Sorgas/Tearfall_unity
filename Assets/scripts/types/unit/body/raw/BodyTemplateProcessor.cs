using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang.extension;

namespace types.unit.body.raw {
    public class BodyTemplateProcessor {
        private static string LEFT_PREFIX = "left ";
        private static string RIGHT_PREFIX = "right ";
        private bool debugMode = false;
        private string logMessage;

        public BodyTemplate process(RawBodyTemplate rawTemplate) {
            logMessage = "";
            log("[BodyTemplateProcessor]: processing " + rawTemplate.name + " body template");
            BodyTemplate template = new BodyTemplate(rawTemplate);
            Dictionary<string, RawBodyPart> rawPartMap = rawTemplate.body.ToDictionary(part => part.name, part => part); // part name to part
            
            // process body parts
            log("    raw parts " + rawPartMap.Count);
            updateLimbsMirroringFlags(rawPartMap);
            Dictionary<string, RawBodyPart> doubledRawPartMap = doubleMirroredParts(rawPartMap);
            fillBodyParts(doubledRawPartMap, template);
            log("    processed parts " + template.body.Count);

            // process slots
            log("    raw slots " + rawTemplate.slots.Length);
            mirrorSlots(template, rawTemplate, rawPartMap);
            log("    processed slots " + template.slots.Count);
            
            Debug.Log(logMessage);
            return template;
        }

        /**
         * Multiplies limbs if they are mirrored.
         */
        private Dictionary<string, RawBodyPart> doubleMirroredParts(Dictionary<string, RawBodyPart> map) {
            Dictionary<string, RawBodyPart> newMap = new Dictionary<string, RawBodyPart>();
            foreach (RawBodyPart part in map.Values) {
                if (part.mirrored) {
                    RawBodyPart leftPart = cloneMirroredPart(map, part, LEFT_PREFIX);
                    newMap.Add(leftPart.name, leftPart);
                    RawBodyPart rightPart = cloneMirroredPart(map, part, RIGHT_PREFIX);
                    newMap.Add(rightPart.name, rightPart);
                } else {
                    newMap.Add(part.name, part);
                }
            }
            return newMap;
        }

        private RawBodyPart cloneMirroredPart(Dictionary<string, RawBodyPart> map, RawBodyPart part, string prefix) {
            RawBodyPart newPart = part.clone(); // copy parts
            newPart.name = prefix + newPart.name; // update name
            if (map[part.root].mirrored) { // root is mirrored
                newPart.root = prefix + newPart.root; // update root links
            }
            return newPart;
        }

        /**
         * Creates body parts and links the between each other.
         */
        private void fillBodyParts(Dictionary<string, RawBodyPart> rawPartsMap, BodyTemplate bodyTemplate) {
            foreach (RawBodyPart rawPart in rawPartsMap.Values) { // create limbs
                bodyTemplate.body.Add(rawPart.name, new BodyPart(rawPart));
            }
            foreach (BodyPart part in bodyTemplate.body.Values) { // link limbs
                if (!rawPartsMap[part.name].root.Equals("body"))
                    part.root = bodyTemplate.body[rawPartsMap[part.name].root];
            }
        }

        /**
         * Observes limbs tree and copies mirroring flags from limbs to their children.
         * There should be only one flag on the path from root to leaf limb.
         */
        private void updateLimbsMirroringFlags(Dictionary<string, RawBodyPart> map) {
            map.Values
                .Where(rawBodyPart => !rawBodyPart.mirrored)
                .Where(rawBodyPart => { // check if some of parent limbs is mirrored
                    RawBodyPart limb = rawBodyPart;
                    while (!limb.root.Equals("body")) { // cycle to the root limb
                        if (map[limb.root].mirrored) return true;
                        limb = map[limb.root]; // go to next limb
                    }
                    return false;
                }).ToList().ForEach(rawBodyPart => rawBodyPart.mirrored = true);
        }

        /**
         * Mirrors slots which use only mirrored parts (boots etc.). 
         * Mirrors only limbs in a slot, if there are non-mirrored limbs in that slot (pants).
         * Side prefixes are added in this method. After mirroring limbs, limbs and slots become consistent.
         * Also mirrors desired slots.
         */
        private void mirrorSlots(BodyTemplate template, RawBodyTemplate rawTemplate, Dictionary<string, RawBodyPart> rawPartMap) {
            foreach (string[] slot in rawTemplate.slots) {
                string slotName = slot[0];
                List<string> slotLimbs = slot.subList(1);
                if (containsOnlyMirroredLimbs(slotLimbs, rawPartMap)) { // create two slots (names are prefixed)
                    log("        slot " + slotName + " gets mirroring");
                    template.slots.Add(LEFT_PREFIX + slotName, slotLimbs.Select(s => LEFT_PREFIX + s).ToList());
                    template.slots.Add(RIGHT_PREFIX + slotName, slotLimbs.Select(s => RIGHT_PREFIX + s).ToList());
                    if (rawTemplate.desiredSlots.Contains(slotName)) {
                        template.desiredSlots.Add(LEFT_PREFIX + slotName);
                        template.desiredSlots.Add(RIGHT_PREFIX + slotName);
                    }
                } else { // some  limbs are single, so mirrored limbs are duplicated within same slot
                    slotLimbs = slotLimbs // copy some limbs with prefixes
                        .SelectMany(s => rawPartMap[s].mirrored ? new[] { LEFT_PREFIX + s, RIGHT_PREFIX + s } : new[] { s })
                        .ToList();
                    template.slots.Add(slotName, slotLimbs);
                    if (rawTemplate.desiredSlots.Contains(slotName)) {
                        template.desiredSlots.Add(slotName);
                    }
                }
            }
        }

        private bool containsOnlyMirroredLimbs(List<string> slotLimbs, Dictionary<string, RawBodyPart> rawPartMap) {
            return slotLimbs.TrueForAll(limb => {
                if(!rawPartMap.ContainsKey(limb)) Debug.LogWarning("raw part map not contains " + limb);
                return rawPartMap[limb].mirrored;
            });
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}