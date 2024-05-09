using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.unit;
using types.unit.skill;
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
        public float currentSpeed; // depends on movement direction
        public float step; // speed is added to this value; when reaches 1, position changes
    }

    // present when action checked, but path is not yet found
    public struct UnitMovementTargetComponent {
        public ActionTarget target;
    }

    // present after finding path
    public struct UnitMovementPathComponent {
        public List<Vector3Int> path;
    }

    public struct UnitVisualComponent {
        public UnitGoHandler handler;
        public Vector3 current; // scene space, current position
        public Vector3 target; // scene space, movement target (next tile in path)
        public int headVariant;
        public int bodyVariant;
        public Vector2Int direction;
        public SpriteOrientations orientation;
    }

    public struct BodyComponent {
        public List<string> bodyParts;
    }

    public struct UnitHealthComponent {
        public string overallStatus;
        public List<string> injures;
    }

    public struct MoodComponent {
        public string status;
        public int value;
        public List<UnitMoodModifier> modifiers;
    }

    public struct UnitJobsComponent {
        public Dictionary<string, int> enabledJobs; // job -> priority

        public List<string> getByPriority(int priority) =>
            enabledJobs.Where(pair => pair.Value == priority)
                .Select(pair => pair.Key).ToList();

        public void changePriority(string job, bool positive) {
            enabledJobs[job] = (enabledJobs[job] + (positive ? 1 : -1) + Jobs.PRIORITIES_COUNT) % Jobs.PRIORITIES_COUNT;
        }
    }

public struct UnitSkillComponent {
    public Dictionary<string, UnitSkill> skills;

    public void addSkill(UnitSkill skill) {
        skills.Add(skill.type.name, skill);
    }
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

    public struct UnitAnimationComponent : IEcsIgnoreInFilter {
        public string animationName; // name of state in animator, all states have transition to delay state
        public float animationDelayMax; // seconds between animation plays 
        public float animationDelay; // current time elapsed, Time.DeltaTime is added each frame
    }

    public struct UnitVisualOnBuildingComponent {
    }

    // overrides regular task assignment to create task with this action instead 
    public struct UnitNextTaskComponent {
        public Action action;
    }

public struct UnitAttributesComponent {
    public UnitIntProperty strength;
    public UnitIntProperty agility;
    public UnitIntProperty endurance;
    public UnitIntProperty intelligence;
    public UnitIntProperty will;
    public UnitIntProperty charisma;
}

public struct UnitPropertiesComponent {
    public float baseMoveSpeed;
    public float moveSpeed;
    public float manipulationSpeed; // for working and combat
}

public struct UnitStatusEffectsComponent {
    public string restNeedEffect; // only one need effect can present at time
    public string hungerNeedEffect;
    public Dictionary<string, UnitStatusEffect> effects;
}

public struct UnitMoodModifier {
    public string name;
    public int value;
    public int remainingTime; // hours
}

public struct UnitDoorUserComponent { } // 

public struct UnitNamesComponent {
    public string professionName;
    public string nickName;
}
}