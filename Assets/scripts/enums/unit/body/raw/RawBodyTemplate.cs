using System.Collections.Generic;

namespace enums.unit.body.raw {
    public class RawBodyTemplate {
        public string name;
        public List<RawBodyPart> body;
        public List<string> desiredSlots = new List<string>();
        public List<List<string>> slots = new List<List<string>>();
    }
}