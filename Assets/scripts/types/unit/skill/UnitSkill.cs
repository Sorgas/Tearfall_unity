namespace types.unit.skill {
public class UnitSkill {
    public Skill type;
    public int value;
    public int exp;
    
    public UnitSkill(Skill type, int value, int exp) {
        this.type = type;
        this.value = value;
        this.exp = exp;
    }

    public float getSpeedChange() => type.speedFactor * value;
}
}