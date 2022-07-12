using System.Collections.Generic;
using Leopotam.Ecs;
using types;
using types.building;
using UnityEngine;

namespace game.model.component.task {
    public struct DesignationComponent {
        public DesignationType type;
    }

    public struct DesignationVisualComponent {
        public SpriteRenderer spriteRenderer;
    }

    public struct DesignationConstructionComponent {
        public ConstructionType type;
        // consumed items:
        public string itemType;
        public int material;
        public int amount;
        
        public string materialVariant; // for visual only
    }

    public struct DesignationBuildingComponent {
        public BuildingType type;
        // consumed items:
        public string itemType;
        public int material;
        public int amount;
        
        public string materialVariant; // for visual only
    }
    
    public struct DesignationItemContainerComponent {
        public List<EcsEntity> items;
    }
}