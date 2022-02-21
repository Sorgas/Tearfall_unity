using System;

namespace enums.unit.body.raw {

    [Serializable]
    public class RawBodyTemplate {
        public string name;
        public RawBodyPart[] body;
        public string[][] slots;
        public string[] desiredSlots;

        public RawBodyTemplate() {
            body = Array.Empty<RawBodyPart>();
            slots = Array.Empty<string[]>();
            desiredSlots = Array.Empty<string>();
        }
    }
}