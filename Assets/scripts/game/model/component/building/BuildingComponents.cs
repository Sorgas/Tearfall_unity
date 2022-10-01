using types;
using types.building;
using UnityEngine;

namespace game.model.component.building {
    public struct BuildingComponent {
        public BuildingType type;
        public Orientations orientation;
    }

    public struct BuildingVisualComponent {
        public GameObject gameObject;
    }

    public struct WorkbenchComponent {
        public string name;
    }
}