using game.model.component.task.order;
using types;
using UnityEngine;

namespace game.model.component.task {
    public struct DesignationComponent {
        public DesignationType type;
        public int priority;
    }

    public struct DesignationVisualComponent {
        public SpriteRenderer spriteRenderer;
    }

    public struct DesignationConstructionComponent {
        public ConstructionOrder order;
    }

    // can be multi-tile
    public struct DesignationBuildingComponent {
        public BuildingOrder order;
    }
}