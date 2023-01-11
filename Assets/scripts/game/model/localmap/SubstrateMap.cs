using System.Collections.Generic;
using UnityEngine;
using util.geometry;

namespace game.model.localmap {
    public class SubstrateMap {
        public readonly Dictionary<Vector3Int, SubstrateCell> cells = new();
        public readonly HashSet<SubstrateCell> activeCells = new();

        public void removeAndActivate(Vector3Int position) {
            cells.Remove(position);
            activeCells.RemoveWhere(cell => cell.position == position);
            foreach (Vector3Int offset in PositionUtil.waterflow) {
                Vector3Int pos = position + offset;
                if (cells.ContainsKey(pos)) {
                    Debug.Log("substrate activated " + pos);
                    SubstrateCell cell = cells[pos];
                    cell.active = true;
                    activeCells.Add(cell);
                }
            }
        }
    }

    public class SubstrateCell {
        public Vector3Int position;
        public int type;
        public bool active; // cell is idle if it cannot grow to nearby tile. 
        //TODO add selected variant

        public SubstrateCell(Vector3Int position, int type, bool active) {
            this.position = position;
            this.type = type;
            this.active = active;
        }

        protected bool Equals(SubstrateCell other) {
            return position.Equals(other.position);
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubstrateCell)obj);
        }
        public override int GetHashCode() {
            return position.GetHashCode();
        }
    }
}