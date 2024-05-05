using UnityEngine;

namespace types.unit.skill {
public class UnitSkill {
    public Skill type;
    public int level;
    public float exp;
    
    public UnitSkill(Skill type, int level, int exp) {
        this.type = type;
        this.level = level;
        this.exp = exp;
    }

    public void addExp(float gain) {
        while (gain > 0 && level < UnitSkills.MAX_VALUE) {
            float toNextLevel = UnitSkills.expValues[level] - exp;
            if (toNextLevel > gain) { // no level up
                exp += gain;
                gain = 0;
            } else { // level up
                gain -= toNextLevel;
                level++;
                exp = 0;
            }
        }
    }
    
    public float getSpeedChange() => type.speedFactor * level;
}
}