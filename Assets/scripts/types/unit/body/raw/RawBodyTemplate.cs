using System;
using System.Collections.Generic;
using types.unit.body.raw;

namespace enums.unit.body.raw {

    [Serializable]
    public class RawBodyTemplate {
        public string name;
        public List<RawBodyPart> body;
        public string[][] slots;
        public string[] desiredSlots;

        public RawBodyTemplate() {
            // body = Array.Empty<RawBodyPart>();
            slots = Array.Empty<string[]>();
            desiredSlots = Array.Empty<string>();
            body = new List<RawBodyPart>();
        }
    }
}