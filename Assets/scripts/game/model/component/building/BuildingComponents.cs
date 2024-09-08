using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using types;
using types.building;
using types.unit;
using UnityEngine;
using util.geometry.bounds;
using static game.model.component.task.order.CraftingOrder.CraftingOrderStatus;

namespace game.model.component.building {

    public struct BuildingComponent {
        public BuildingType type;
        public Orientations orientation;
        public IntBounds3 bounds;
        
        public Vector3Int getAccessPosition(Vector3Int pos) {
            return type.getAccessByPositionAndOrientation(pos, orientation);
        }
    }

    public struct BuildingVisualComponent {
        public GameObject gameObject;
    }

    // indicates that building is a workbench
    public struct WorkbenchComponent {
        public List<CraftingOrder> orders;
        public bool hasActiveOrders; // 
        public bool updatedFromModel;
        public Job job;
        
        public void update() {
            hasActiveOrders = orders.Count != 0 && orders.Count(order => order.status != PAUSED) != 0;
        }
    }

    // units can sleep on building with this component. quality affects sleep speed and mood buff
    public struct BuildingSleepFurnitureC {
        public float quality;
    }

    // units can sit on building with this component. quality affects mood buff
    public struct BuildingSitFurnitureC {
        public float quality;
        public Orientations orientation;
    }

    public struct BuildingTableFurnitureC {}

public struct BuildingDoorComponent {
    public float openness;
    public float openingSpeed;
    public float closingSpeed;
    public float timeout; // closing timeout
    // public const float maxTimeout = 3;
}

public struct BuildingDoorOpenComponent { }

public struct EnemyBuildingComponent { }
}