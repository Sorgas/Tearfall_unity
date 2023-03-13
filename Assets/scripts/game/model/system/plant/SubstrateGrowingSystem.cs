using System.Collections.Generic;
using game.model.localmap;
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

        public SubstrateGrowingSystem(LocalModel model) : base(model, 500) { }

        public override void runLogic() {
            SubstrateMap map = model.localMap.substrateMap;
            if (map.hasActive()) return;
            HashSet<SubstrateCell> toAdd = new();
            HashSet<Vector3Int> toDeactivate = new();
            ICollection<SubstrateCell> list = map.selectNRandomActiveCells(0.1f);
            foreach (SubstrateCell cell in list) {
                Vector3Int target = checkTilesAround(cell);
                if (target != notFoundValue) {
                    toAdd.Add(new SubstrateCell(target, cell.type, true));
                } else {
                    toDeactivate.Add(cell.position);
                }
            }

            foreach (Vector3Int position in toDeactivate) {
                map.deactivate(position);
            }

            foreach (SubstrateCell cell in toAdd) {
                map.add(cell.position, cell.type, cell.active);
            }
        }

        private Vector3Int checkTilesAround(SubstrateCell cell) {
            SubstrateType type = SubstrateTypeMap.get().get(cell.type);
            foreach (Vector3Int offset in PositionUtil.waterflow) { // TODO add different orders for iteration
                Vector3Int target = cell.position + offset;
                if (!model.localMap.substrateMap.has(target)
                    && model.localMap.blockType.get(target) != BlockTypes.SPACE.CODE) {
                    Material_ blockMaterial = MaterialMap.get().material(model.localMap.blockType.getMaterial(target));
                    // if (type.placement.Contains("light") && model.localMap.substrateMap.) {
                    //
                    // }
                    if (blockMaterial.tags.Contains(type.blockTag)) return target;
                }
            }
            return notFoundValue;
        }
    }
}