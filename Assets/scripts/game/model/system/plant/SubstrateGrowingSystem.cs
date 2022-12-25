using System;
using System.Collections.Generic;
using game.view;
using types;
using types.material;
using types.plant;
using UnityEngine;
using util.geometry;
using Random = UnityEngine.Random;

namespace game.model.system.plant {
    // spreads substrates to nearby available tiles
    // TODO activate substrates on digging, animal pastures, fires etc.
    public class SubstrateGrowingSystem : LocalModelIntervalSystem {
        private static Vector3Int notFoundValue = Vector3Int.back;

        public SubstrateGrowingSystem(LocalModel model) : base(model, 500) { }

        public override void runLogic() {
            HashSet<SubstrateCell> toAdd = new();
            HashSet<SubstrateCell> toRemove = new();
            SubstrateMap map = model.localMap.substrateMap;
            if (map.activeCells.Count == 0) return;
            ICollection<SubstrateCell> list = selectNRandom(map.activeCells, (int)Math.Ceiling(map.activeCells.Count / 10f));
            foreach (SubstrateCell cell in list) {
                Vector3Int target = checkTilesAround(cell);
                if (target != notFoundValue) {
                    toAdd.Add(new SubstrateCell(target, cell.type, true));
                } else {
                    cell.active = false;
                    toRemove.Add(cell);
                }
            }

            foreach (SubstrateCell cell in toRemove) {
                map.activeCells.Remove(cell);
            }

            foreach (SubstrateCell cell in toAdd) {
                map.cells.Add(cell.position, cell);
                map.activeCells.Add(cell);
                GameView.get().tileUpdater.updateTile(cell.position, false);
            }
        }

        private Vector3Int checkTilesAround(SubstrateCell cell) {
            SubstrateType type = SubstrateTypeMap.get().get(cell.type);
            foreach (Vector3Int offset in PositionUtil.waterflow) { // TODO add different orders for iteration
                Vector3Int target = cell.position + offset;
                if (!model.localMap.substrateMap.cells.ContainsKey(target)
                    && model.localMap.blockType.get(target) != BlockTypes.SPACE.CODE) {
                    Material_ blockMaterial = MaterialMap.get().material(model.localMap.blockType.getMaterial(target));
                    if (blockMaterial.tags.Contains(type.blockTag)) return target; // 
                }
            }
            return notFoundValue;
        }

        private ICollection<SubstrateCell> selectNRandom(ICollection<SubstrateCell> source, int targetNumber) {
            if (targetNumber >= source.Count) return source;
            List<SubstrateCell> result = new();
            int i = 0;
            foreach (SubstrateCell cell in source) {
                if (Random.Range(0, 1) < (targetNumber - result.Count) / (source.Count - i)) {
                    result.Add(cell);
                }
                if (result.Count == targetNumber) return result;
                i++;
            }
            return result;
        }
    }
}