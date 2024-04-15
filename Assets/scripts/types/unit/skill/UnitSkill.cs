namespace game.model.component.unit {
public class UnitSkill {
    public string name;
    public int value;
    public int exp;
    public int priority;
    
    public UnitSkill(string name, int value, int exp) {
        this.name = name;
        this.value = value;
        this.exp = exp;
    }
}
}