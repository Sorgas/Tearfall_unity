using System.Collections.Generic;
using enums;
using enums.action;
using game.model.component.task.action;
using UnityEngine;

namespace game.model.component.unit.components { // entity with this is a unit
    public struct UnitComponent { }

// stores unit's position, task target position and path to target
    public struct MovementComponent {
        public Vector3Int position; // model position
        public ActionTargetTypeEnum targetType; // near/exact/any
        public List<Vector3Int> path;
        public OrientationEnum orientation;
        public float speed;
        public float step; // speed is added to this value; when reaches 1, position changed
    }

    public struct MovementTargetComponent {
        public Vector3Int target;
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

    public struct TestComponent {
        public string value;
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

// unit has this when task assigned
    public struct TaskComponent {
        public _Action initialAction;
        public List<_Action> preActions;

        public _Action getNextAction() {
            if (preActions.Count > 0) {
                return preActions[0];
            }
            return initialAction;
        }

        public void addFirstPreAction(_Action action) {
            preActions.Insert(0, action);
        }

        public void removeFirstPreAction() {
            preActions.RemoveAt(0);
        }

        public string toString() {
            return initialAction.ToString();
        }
    }

// unit with this is performing action
    public struct CurrentActionComponent {
        public _Action action;
    }
}