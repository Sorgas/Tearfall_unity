using System;

namespace types.unit.body.raw {
    
    [Serializable]
    public class RawBodyPart {
        public string name; // id
        public string root; // points to limb current limb is attached to
        public bool mirrored;
        public string[] layers;
        public string[] internalOrgans;
        public string[] externalOrgans;
        public string[] tags;

        // for json reader
        public RawBodyPart() { }

        public RawBodyPart clone() {
            RawBodyPart clone = new RawBodyPart();
            clone.name = name;
            clone.root = root;
            clone.mirrored = mirrored;
            if(layers != null) {
                clone.layers = new string[layers.Length];
                Array.Copy(layers, clone.layers, layers.Length);
            }
            if (tags != null) {
                clone.tags = new string[tags.Length];
                Array.Copy(tags, clone.tags, tags.Length);
            }
            if(internalOrgans != null) {
                clone.internalOrgans = new string[internalOrgans.Length];
                Array.Copy(internalOrgans, clone.internalOrgans, internalOrgans.Length);
            }
            if(externalOrgans != null) {
                clone.externalOrgans = new string[externalOrgans.Length];
                Array.Copy(externalOrgans, clone.externalOrgans, externalOrgans.Length);
            }
            return clone;
        }
    }
}