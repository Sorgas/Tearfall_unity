using System.Collections.Generic;
using game.view;
using types;
using types.material;
using types.plant;
using UnityEngine;
using util.geometry;

namespace game.model.system.plant {
    // spreads substrates to nearby available tiles
    // TODO activate substrates on digging, animal pastures, fires etc.
    public class SubstrateGrowingSystem : LocalModelIntervalSystem {
        private static Vector3Int notFoundValue = Vector3Int.back;
        
        public SubstrateGrowingSystem(LocalModel model) : base(model, 5) { }

        public override void runLogic() {
            HashSet<SubstrateCell> toAdd = new();
            HashSet<SubstrateCell> toRemove = new();
            SubstrateMap map = model.localMap.substrateMap;
            if (map.activeCells.Count == 0) return;
            foreach (SubstrateCell cell in map.activeCells) {
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
            foreach (Vector3Int target in PositionUtil.waterflow) { // TODO add different orders for iteration
                if(!model.localMap.substrateMap.cells.ContainsKey(target) 
                   && model.localMap.blockType.get(target) != BlockTypes.SPACE.CODE) {
                    Material_ blockMaterial = MaterialMap.get().material(model.localMap.blockType.getMaterial(target));
                    if (blockMaterial.tags.Contains(type.blockTag)) return target; // 
                }
            }
            return notFoundValue;
        }
    }
}