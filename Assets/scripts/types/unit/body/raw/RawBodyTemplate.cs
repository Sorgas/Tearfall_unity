using System;
using System.Collections.Generic;

namespace types.unit.body.raw {
[Serializable]
public class RawBodyTemplate {
    public string name;
    public List<RawBodyPart> body;
    public SlotDefinition[] slots;
    public SlotDefinition[] grabSlots;
    public string[] desiredSlots;

    public RawBodyTemplate() {
        slots = Array.Empty<SlotDefinition>();
        slots = Array.Empty<SlotDefinition>();
        grabSlots = Array.Empty<SlotDefinition>();
        desiredSlots = Array.Empty<string>();
        body = new List<RawBodyPart>();
    }
}

[Serializable]
public class SlotDefinition {
    public string name;
    public string[] limbs;
    public bool canMirror = true;
}
}