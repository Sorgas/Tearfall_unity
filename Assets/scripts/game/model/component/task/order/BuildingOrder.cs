using types;
using types.building;
using UnityEngine;

namespace game.model.component.task.order {
    public class BuildingOrder : GenericBuildingOrder {
        public BuildingType type;
        public Orientations orientation;
        
        public BuildingOrder(string itemType, int material, int amount, Vector3Int position) : base(itemType, material, amount, position) {
        }
    }
}