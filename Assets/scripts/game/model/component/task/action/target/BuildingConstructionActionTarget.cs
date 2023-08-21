using System.Collections.Generic;
using game.model.localmap;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
    // action target for building buildings
    public class BuildingConstructionActionTarget : ActionTarget {
        public readonly Vector3Int center;
        public Vector3Int builderPosition;

        public BuildingConstructionActionTarget(Vector3Int position) : base(ActionTargetTypeEnum.EXACT) {
            center = position;
            // TODO access position if type has any, or any near tile
        }

        public override Vector3Int pos => center;
        
        public override List<Vector3Int> getPositions(LocalModel model) {
            throw new System.NotImplementedException();
        }
        // return builderPosition != null ? builderPosition : center;
        // /**
        //  * Finds appropriate position for builder to build from and bring materials.
        //  * TODO find nearest position.
        //  */
        // public bool findPositionForBuilder(BuildingOrder order, Vector3Int currentBuilderPosition) {
        //     UtilByteArray area = GameModel.localMap.passageMap.area;
        //     if (order.blueprint.construction) {
        //         BlockType blockType = BlockTypes.get(order.blueprint.building);
        //         builderPosition = new NeighbourPositionStream(order.position) // position near target tile
        //             .filterInArea(area.get(currentBuilderPosition))
        //             .filterByAccessibilityWithFutureTile(blockType)
        //             .stream.findFirst().orElse(null);
        //     } else {
        //         BuildingType type = BuildingTypeMap.getBuilding(order.blueprint.building);
        //         Int2dBounds bounds = new Int2dBounds(order.position, RotationUtil.orientSize(type.size, order.orientation));
        //         bounds.extend(1);
        //         int builderArea = area.get(currentBuilderPosition);
        //         builderPosition = bounds.collectBorders().stream() // near position with same area
        //             .map(vector -> new IntVector3(vector.x, vector.y, order.position.z))
        //             .filter(position -> area.get(position) == builderArea)
        //             .findFirst().orElse(null);
        //     }
        //     if (builderPosition != null) type = EXACT;
        //     return builderPosition != null;
        // }
        //
        // public void reset() {
        //     builderPosition = null;
        //     type = NEAR;
        // }
    }
}