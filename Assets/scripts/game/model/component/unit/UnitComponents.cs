using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.unit;
using types.unit.body;
using types.unit.disease;
using types.unit.skill;
using UnityEngine;
using util.lang;

namespace game.model.component.unit {

    // entity with this is a unit
    public struct UnitComponent {
        public CreatureType type;
        public string sex;
    }

    // present when action checked, but path is not yet found
    public struct UnitMovementTargetComponent {
        public ActionTarget target;
    }

    // present after pathfinding, while unit is moving
    public struct UnitMovementComponent {
        public List<Vector3Int> path;
        public float currentSpeed; // depends on movement direction
        public float step; // speed is added to this value; when reaches 1, position changes
    }

    public struct UnitVisualComponent {
        public UnitGoHandler handler;
        public Vector3 current; // scene space, current position
        public Vector3 target; // scene space, movement target (next tile in path)
        public int headVariant;
        public int bodyVariant;
        public Vector2Int direction;
        public UnitOrientations orientation;
        // public bool orientationChanged; // to be raised only when orientation changes

        // public void setOrientation(UnitOrientations orientation) {
        //     if(orientation != this.orientation)
        // }
    }

    public struct BodyComponent {
        public List<string> bodyParts;
    }

    public struct UnitHealthComponent {
        public string overallStatus;
        public MultiValueDictionary<string, Injury> injuries; // body 
        public float stamina;
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

// if action has animation specified, this component is created along with UnitCurrentActionComponent
public struct UnitCurrentAnimatedActionComponent {
    public string animationName; 
}

    public struct UnitSleepingComponent {
    }

// present when unit is performing action which requires display of progress bar
public struct UnitProgressBarComponent { }

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

public struct UnitPropertiesComponent {
    public Dictionary<string, UnitIntProperty> attributes;
    public Dictionary<string, UnitFloatProperty> properties;
}

public struct UnitStatusEffectsComponent {
    public string restNeedEffect; // only one need effect can present at time
    public string hungerNeedEffect;
    public HashSet<string> effects;
}

public struct UnitDiseaseComponent {
    public Dictionary<string, UnitDisease> diseases;
}

public struct UnitDoorUserComponent { } // 

public struct UnitNamesComponent {
    public string professionName;
    public string nickName;
}

// when present, unit will not receive tasks from container or needs
public struct UnitDraftedComponent {
    
}

// when unit falls unconscious by any reason (e.g. combat) this component is present
public struct UnitDownedComponent {
    
}

// after attack, this component is added to unit
public struct UnitAttackCooldownComponent {
    public int ticks;
}

public struct CorpseComponent { }

public struct UnitCombatComponent {
    public float blockingCooldown;
    public float parryCooldown;
    public float dodgeCooldown;
}
}