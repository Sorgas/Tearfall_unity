using System.Collections.Generic;
using enums.unit.body.raw;

namespace enums.unit.body {
    public class BodyPart {
        //determine wear items, that can be equipped (slot name = side + type)
        public string name; //

        public BodyPart root; // each body part points to one it`s connected to
        public readonly List<string> layers = new List<string>(); // tissue layers
        public readonly List<string> externalOrgans = new List<string>();
        public readonly List<string> internalOrgans = new List<string>();
        public readonly List<string> tags = new List<string>();

        public BodyPart(RawBodyPart rawBodyPart) {
            name = rawBodyPart.name;
            layers.AddRange(rawBodyPart.layers);
            internalOrgans.AddRange(rawBodyPart.internalOrgans);
            externalOrgans.AddRange(rawBodyPart.externalOrgans);
            tags.AddRange(rawBodyPart.tags);
        }

        public BodyPart() {}
    
        public BodyPart clone() {
            BodyPart bodyPart = new BodyPart();
            bodyPart.name = name;
            bodyPart.root = root;
            bodyPart.tags.AddRange(tags);
            bodyPart.layers.AddRange(layers);
            bodyPart.internalOrgans.AddRange(internalOrgans);
            bodyPart.externalOrgans.AddRange(externalOrgans);
            return bodyPart;
        }
    }
}