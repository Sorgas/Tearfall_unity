using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.action;
using types.unit;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace game.model.component.unit {

    // entity with this is a unit
    public struct UnitComponent {
        public CreatureType type;
        public string sex;
    }

    // stores unit's movement properties
    public struct UnitMovementComponent {
        public float speed; // tiles per model tick
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
        public Vector3 speed; // sprite moving speed
        public Vector3 current; // current position
        public Vector3 target; // movement target
        public int headVariant;
        public int bodyVariant;
        public SpriteOrientations orientation;
    }

    public struct BodyComponent {
        public List<string> bodyParts;
    }

    public struct HealthComponent {
        public string overallStatus;
        public List<string> injures;
    }

    public struct MoodComponent {
        public string status;
    }

    public struct UnitJobsComponent {
        public Dictionary<string, int> enabledJobs; // job -> priority

        public List<string> getByPriority(int priority) =>
            enabledJobs.Where(pair => pair.Value == priority)
                .Select(pair => pair.Key).ToList();
    }

    public struct OwnershipComponent {
        public string wealthStatus;
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

    public struct UnitSleepingComponent {
    }

    public struct UnitVisualProgressBarComponent : IEcsIgnoreInFilter {
    }

    public struct UnitVisualOnBuildingComponent {
    }

    // overrides regular task assignment to create task with this action instead 
    public struct UnitNextTaskComponent {
        public Action action;
    }

public struct UnitAttributesComponent {
    public int strength;
    public int agility;
    public int endurance;
    public int intelligence;
    public int will;
    public int charisma;
}
}