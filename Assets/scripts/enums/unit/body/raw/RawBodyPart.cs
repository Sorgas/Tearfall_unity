using System;

namespace enums.unit.body.raw {
    
    [Serializable]
    public class RawBodyPart {
        public string name;
        public string root;
        public bool mirrored; // bi for left and right, quadro for front left, rear right, etc todo
        public readonly string[] layers;
        public readonly string[] internalOrgans;
        public readonly string[] externalOrgans;
        public readonly string[] tags;

        // for json reader
        public RawBodyPart() { }

        public RawBodyPart clone() {
            RawBodyPart clone = new RawBodyPart();
            clone.name = name;
            clone.root = root;
            clone.mirrored = mirrored;
            if(layers != null) Array.Copy(layers, clone.layers, layers.Length);
            if(tags != null) Array.Copy(tags, clone.tags, tags.Length);
            if(internalOrgans != null) Array.Copy(internalOrgans, clone.internalOrgans, internalOrgans.Length);
            if(externalOrgans != null) Array.Copy(externalOrgans, clone.externalOrgans, externalOrgans.Length);
            return clone;
        }
    }
}