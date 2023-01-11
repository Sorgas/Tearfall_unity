using System.Collections.Generic;
using types.unit.body.raw;

namespace types.unit.body {
    public class BodyTemplate {
        public string name;
        public Dictionary<string, BodyPart> body; // name to bodyPart
        public Dictionary<string, List<string>> slots; // slot name to default limbs
        public List<string> desiredSlots;

        public BodyTemplate(RawBodyTemplate rawBodyTemplate) {
            name = rawBodyTemplate.name;
            body = new Dictionary<string, BodyPart>();
            slots = new Dictionary<string, List<string>>();
            desiredSlots = new List<string>();
        }
    }
}