using System.Collections.Generic;
using types.action;
using types.unit.need;

namespace game.model.component.unit {
    // stores values of unit's needs. values are [0..1]f, more means satisfied.
    public struct UnitNeedComponent {
        public float hunger;
        public float thirst;
        public float rest;
        public int hungerPriority;
        public int thirstPriority;
        public int restPriority;
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
        public int priority;
    }

    public struct NeedState {
        
    }
}