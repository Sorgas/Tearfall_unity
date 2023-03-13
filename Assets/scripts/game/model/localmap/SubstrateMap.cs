using System;
using System.Collections.Generic;
using UnityEngine;
using util.geometry;
using Random = UnityEngine.Random;

namespace game.model.localmap {
    public class SubstrateMap : LocalModelUpdateComponent {
        private readonly Dictionary<Vector3Int, SubstrateCell> cells = new();
        private readonly HashSet<SubstrateCell> activeCells = new();

        public SubstrateMap(LocalModel model) : base(model) { }

        public void add(Vector3Int position, int type, bool active) {
            SubstrateCell cell = new(position, type, active);
            cells.Add(position, cell);
            if (active) activeCells.Add(cell);
            addPositionForUpdate(position);
        }

        public void removeAndActivate(Vector3Int position) {
            remove(position);
            activateAround(position);
        }

        public void remove(Vector3Int position) {
            cells.Remove(position);
            activeCells.RemoveWhere(cell => cell.position == position);
            addPositionForUpdate(position);
        }

        public void deactivate(Vector3Int position) {
            cells.Remove(position);
            activeCells.RemoveWhere(cell => cell.position == position);
        }

        public bool has(Vector3Int position) => cells.ContainsKey(position);

        public SubstrateCell get(Vector3Int position) => cells[position];

        public ICollection<SubstrateCell> selectNRandomActiveCells(float percentage) {
            int targetNumber = (int)Math.Ceiling(activeCells.Count * percentage);
            if (targetNumber >= activeCells.Count) return activeCells;
            List<SubstrateCell> result = new();
            int i = 0;
            foreach (SubstrateCell cell in activeCells) {
                if (Random.Range(0, 1) < (targetNumber - result.Count) / (activeCells.Count - i)) {
                    result.Add(cell);
                }
                if (result.Count == targetNumber) return result;
                i++;
            }
            return result;
        }

        public bool hasActive() {
            return activeCells.Count > 0;
        }
        
        private void activateAround(Vector3Int position) {
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