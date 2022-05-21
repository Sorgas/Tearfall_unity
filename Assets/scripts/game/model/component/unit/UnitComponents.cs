using System.Collections.Generic;
using enums;
using enums.action;
using game.model.component.task.action;
using game.view.util;
using UnityEngine;

namespace game.model.component.unit { 
    
    // entity with this is a unit
    public struct UnitComponent {
        public int id;
    }
    
    // stores unit's movement properties
    public struct UnitMovementComponent {
        // public Vector3Int position; // model position
        public OrientationEnum orientation;
        public float speed;
        public float step; // speed is added to this value; when reaches 1, position changed
        
    }

    public struct UnitMovementTargetComponent {
        public Vector3Int target;
        public ActionTargetTypeEnum targetType; // near/exact/any
    }

    public struct UnitMovementPathComponent {
        public List<Vector3Int> path;
    }
    
    public struct UnitVisualComponent {
        public UnitGoHandler handler;
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

    public struct UnitJobsComponent {
        // TODO add priorities as {job, priority} entry. handle in taskcontainer and task assignment system
        public List<string> enabledJobs;
    }

    public struct OwnershipComponent {

    }

// stores body temperature
    public struct TemperatureComponent {
        public float value;
    }

    public struct AgeComponent {
        public int age;
    }

    // unit with this is performing action
    public struct UnitCurrentActionComponent {
        public Action action;
    }
}