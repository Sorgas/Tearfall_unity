using System.Collections.Generic;
using enums;
using enums.action;
using game.model.component.task.action;
using UnityEngine;

namespace game.model.component.unit { 
    
    // entity with this is a unit
    public struct UnitComponent {
        public int id;
    }
    
    // stores unit's position, task target position and path to target
    public struct MovementComponent {
        public Vector3Int position; // model position
        public List<Vector3Int> path;
        public OrientationEnum orientation;
        public float speed;
        public float step; // speed is added to this value; when reaches 1, position changed
    }

    public struct MovementTargetComponent {
        public Vector3Int target;
        public ActionTargetTypeEnum targetType; // near/exact/any
    }

    public struct UnitVisualComponent {
        public SpriteRenderer spriteRenderer;
    }

    public struct EquipmentComponent {
        public List<string> slots;
    }

    public struct BodyComponent {
        public List<string> bodyParts;
    }

    public struct HealthComponent {
        public List<string> injures;
    }

    public struct JobsComponent {
        public List<string> enabledJobs;
    }

    public struct OwnershipComponent {

    }

// stores body temperature
    public struct TemperatureComponent {
        public float value;
    }

    public struct UnitNameComponent {
        public string name;
    }

    public struct AgeComponent {
        public int age;
    }

    // unit with this is performing action
    public struct CurrentActionComponent {
        public Action action;
    }
}