using game.model.system.unit;

namespace types.unit.disease {
// Disease caused by some substantial cause, like infection or poison. 
public class CausedDisease : Disease {
    public float hoursToHeal = -1; // set for diseases healed by unit's body, e.g. diseases and poisons
    public float healingDelta;

    public CausedDisease(string name, bool lethal, float hoursToKill, float hoursToHeal, string description, string notificationText, string notificationIcon)
        : base(name, lethal, hoursToKill, description, notificationText, notificationIcon) {

        this.hoursToHeal = hoursToHeal;
        healingDelta = UnitDiseaseSystem.delta / hoursToHeal;
    }
}
}