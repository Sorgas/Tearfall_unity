using types;
using types.building;
using UnityEngine;

namespace game.model.component.task.order {
    public class BuildingOrder : GenericBuildingOrder {
        public BuildingType type;
        public Orientations orientation;

        public BuildingOrder(Vector3Int position, BuildingType type, Orientations orientation) : base(position) {
            this.type = type;
            this.orientation = orientation;
        }

        public BuildingOrder(BuildingOrder source) : base(source) {
            type = source.type;
            orientation = source.orientation;
        }
    }
}