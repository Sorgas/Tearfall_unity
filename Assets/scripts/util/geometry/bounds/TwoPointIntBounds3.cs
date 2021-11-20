using UnityEngine;

namespace util.geometry {
    public class TwoPointIntBounds3 : IntBounds3 {
        private Vector3Int _pos1;
        private Vector3Int _pos2;

        public Vector3Int pos1 {
            get => _pos1;
            set {
                _pos1 = value;
                set(_pos1, _pos2);
            }
        }

        public Vector3Int pos2 {
            get => _pos2;
            set {
                _pos2 = value;
                set(_pos1, _pos2);
            }
        }

        public TwoPointIntBounds3() : this(new Vector3Int(), new Vector3Int()) { }

        public TwoPointIntBounds3(Vector3Int pos1, Vector3Int pos2) {
            _pos1 = pos1;
            _pos2 = pos2;
        }

        public void set(Vector3Int pos1, Vector3Int pos2) {
            base.set(pos1, pos2);
            _pos1 = pos1;
            _pos2 = pos2;
        }

        public TwoPointIntBounds3 set(TwoPointIntBounds3 source) {
            base.set(source);
            return this;
        }
    }
}