using System.Collections.Generic;
using enums.action;
using enums.unit.need;

namespace game.model.component.unit {
    public struct UnitNeedComponent {
        public int hunger;
        public int thirst;
        public int sleep;

        // filled in UnitNeedSystem
        // tasks for this needs created in UnitTaskAssignmentSystem
        public Dictionary<Need, TaskPriorityEnum> needsToFullfill;
    }

    // is present, if unit needs wear on some slots 
    public struct UnitWearNeedComponent {
        public bool valid; // when equipment changes, this component invalidates
        public List<string> desiredSlots; // slots required to be filled by creature type
    }

    
    
    // is present, if required slots are empty
    public struct UnitCalculatedWearNeedComponent {
        public List<string> slotsToFill; // currently empty slots (if maxNeed is WEAR)
    }

    public struct UnitCalculatedNeedComponent {
        public Need need;
        public TaskPriorityEnum priority;
    }
}