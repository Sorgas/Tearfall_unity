using System.Collections.Generic;

namespace enums.unit.body.raw {
    public class RawBodyPart {
        public string name;
        public string root;
        public bool mirrored; // bi for left and right, quadro for front left, rear right, etc todo
        public readonly List<string> layers;
        public readonly List<string> internalOrgans;
        public readonly List<string> externalOrgans;
        public readonly List<string> tags;

        // for json reader
        public RawBodyPart() {
            internalOrgans = new List<string>();
            externalOrgans = new List<string>();
            tags = new List<string>();
            layers = new List<string>();
        }

        public RawBodyPart clone() {
            RawBodyPart clone = new RawBodyPart();
            clone.name = name;
            clone.root = root;
            clone.mirrored = mirrored;
            clone.layers.AddRange(layers);
            clone.tags.AddRange(tags);
            clone.internalOrgans.AddRange(internalOrgans);
            clone.externalOrgans.AddRange(externalOrgans);
            return clone;
        }
    }
}