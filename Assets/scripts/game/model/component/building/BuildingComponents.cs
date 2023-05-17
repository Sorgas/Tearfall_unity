using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using Leopotam.Ecs;
using types;
using types.building;
using types.unit;
using UnityEngine;

namespace game.model.component.building {

    public struct BuildingComponent {
        public BuildingType type;
        public Orientations orientation;
    }

    public struct BuildingVisualComponent {
        public GameObject gameObject;
    }

    // indicates that building is a workbench
    public struct WorkbenchComponent {
        public List<CraftingOrder> orders;
        public bool hasActiveOrders;
        public Job job; 
        
        public void updateFlag() {
            hasActiveOrders = orders.Count != 0 && orders.Where(order => !order.paused).Count() != 0;
        }
    }

    // // if WB has orders, this component is present
    // public struct WorkbenchOrdersComponent {
    // }

    public struct WorkbenchCurrentOrderComponent {
        public CraftingOrder currentOrder;
    }

    public struct BuildingItemContainerComponent {
        public List<EcsEntity> items;
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
}