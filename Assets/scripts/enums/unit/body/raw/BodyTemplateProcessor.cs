using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace enums.unit.body.raw {
    public class BodyTemplateProcessor {
        private static string LEFT_PREFIX = "left ";
        private static string RIGHT_PREFIX = "right ";
        private bool debugMode = true;

        public BodyTemplate process(RawBodyTemplate rawTemplate) {
            BodyTemplate template = new BodyTemplate(rawTemplate);
            log("processing " + rawTemplate.name + " body template");
            Dictionary<string, RawBodyPart> rawPartMap = rawTemplate.body
                .ToDictionary(part => part.name, part => part); // part name to part
            log("    rawPartMap " + rawPartMap.Count + " " + rawPartMap.Keys);
            updateLimbsMirroringFlags(rawPartMap);
            log("    unmirrored slots " + rawTemplate.slots.Count);
            mirrorSlots(rawTemplate, rawPartMap);
            foreach (var slot in rawTemplate.slots) { // copy slots to new template
                template.slots.Add(slot[0], slot.GetRange(1, slot.Count - 1));
            }
            log("    mirrored slots " + template.slots.Count + " " + template.slots.Keys);
            doubleMirroredParts(rawPartMap);
            log("    doubled parts " + rawPartMap.Count + " " + rawPartMap.Keys);
            fillBodyParts(rawPartMap, template);
            return template;
        }

        /**
         * Mirrors slots which use only mirrored parts (boots etc.). 
         * Mirrors only limbs in a slot, if there are non-mirrored limbs in that slot (pants).
         * Side prefixes are added in this method. After mirroring limbs, limbs and slots become consistent.
         * Also mirrors desired slots.
         */
        private void mirrorSlots(RawBodyTemplate rawTemplate, Dictionary<string, RawBodyPart> rawPartMap) {
            List<List<string>> newSlots = new List<List<string>>();
            foreach (List<string> slot in rawTemplate.slots) {
                string slotName = slot[0];
                List<string> slotLimbs = slot.GetRange(1, slot.Count - 1);
                if (containsOnlyMirroredLimbs(slotLimbs, rawPartMap)) { // create two slots (names are prefixed)
                    log("        slot " + slotName + " gets mirroring");
                    newSlots.Add(slot.Select(s => LEFT_PREFIX + s).ToList()); // left copy of a slot
                    newSlots.Add(slot.Select(s => RIGHT_PREFIX + s).ToList()); // right copy of a slot
                    if (rawTemplate.desiredSlots.Contains(slotName)) {
                        rawTemplate.desiredSlots.Remove(slotName);
                        rawTemplate.desiredSlots.Add(LEFT_PREFIX + slotName);
                        rawTemplate.desiredSlots.Add(RIGHT_PREFIX + slotName);
                    }
                } else { // some limbs are single, so mirrored limbs are duplicated in same slot
                    int notMirroredLimbsSize = slotLimbs.Count;
                    List<string> newSlotLimbs = slotLimbs // copy some limbs with prefixes
                        .SelectMany(s => rawPartMap[s].mirrored ? new[] {LEFT_PREFIX + s, RIGHT_PREFIX + s} :new[] {s})
                        .ToList();
                    if (newSlotLimbs.Count > notMirroredLimbsSize)
                        log("        " + (newSlotLimbs.Count - notMirroredLimbsSize) + " limb(s) got mirrored in slot " + slotName);
                    newSlotLimbs.Insert(0, slotName);
                    newSlots.Add(newSlotLimbs);
                }
            }
            rawTemplate.slots = newSlots;
        }

        /**
         * Multiplies limbs if they are mirrored.
         */
        private void doubleMirroredParts(Dictionary<string, RawBodyPart> map) {
            // double mirrored parts
            Dictionary<string, RawBodyPart> newMap = new Dictionary<string, RawBodyPart>();
            foreach (RawBodyPart part in map.Values) {
                if (part.mirrored) {
                    RawBodyPart leftPart = part.clone(); // copy parts
                    RawBodyPart rightPart = part.clone();
                    leftPart.name = LEFT_PREFIX + leftPart.name; // update name
                    rightPart.name = RIGHT_PREFIX + rightPart.name;
                    if (map[part.root].mirrored) { // root is mirrored
                        leftPart.root = LEFT_PREFIX + leftPart.root; // update root links
                        rightPart.root = RIGHT_PREFIX + rightPart.root;
                    }
                    newMap.Add(leftPart.name, leftPart);
                    newMap.Add(rightPart.name, rightPart);
                } else {
                    newMap.Add(part.name, part);
                }
            }
            map.Clear();
            foreach (var entry in newMap) {
                map.Add(entry.Key, entry.Value);
            }
        }

        private bool containsOnlyMirroredLimbs(List<string> slotLimbs, Dictionary<string, RawBodyPart> rawPartMap) {
            return slotLimbs.TrueForAll(limb => rawPartMap[limb].mirrored);
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
         * Creates body parts and links the between each other.
         */
        private void fillBodyParts(Dictionary<string, RawBodyPart> rawPartsMap, BodyTemplate bodyTemplate) {
            foreach (RawBodyPart rawPart in rawPartsMap.Values) { // create limbs
                bodyTemplate.body.Add(rawPart.name, new BodyPart(rawPart));
            }
            foreach (BodyPart part in bodyTemplate.body.Values) { // link limbs
                part.root = bodyTemplate.body[rawPartsMap[part.name].root];
            }
        }

        private void log(string message) {
            Debug.Log(message);
        }
    }
}