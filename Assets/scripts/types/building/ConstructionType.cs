using System;

namespace types.building {
    [Serializable]
    public class ConstructionType {
        public string name;
        public string blockTypeName;
        public string[] materials;
        public ConstructionVariant[] variants;
        
        public BlockType blockType;
    }

    public class ConstructionVariant : IEquatable<ConstructionVariant> {
        public ConstructionVariant(string raw) {
            string[] args = raw.Split("/");
            itemType = args[0];
            amount = int.Parse(args[1]);
        }

        public string itemType;
        public int amount;

        public bool Equals(ConstructionVariant other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return itemType == other.itemType && amount == other.amount;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConstructionVariant)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(itemType, amount);
        }
    }
}