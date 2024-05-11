namespace game.model.component.unit {
public class UnitFloatProperty {
    public readonly float baseValue; // depends on a creature race;
    public float value;

    public UnitFloatProperty(float value) {
        baseValue = value;
        this.value = value;
    }
}
}