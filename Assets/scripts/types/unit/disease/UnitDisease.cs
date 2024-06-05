using Unity.Mathematics;

namespace types.unit.disease {
public class UnitDisease {
    public Disease type;
    public float progress; // [0..1]
    public float healProgress; // [0..1]
    
    public UnitDisease(Disease type) {
        this.type = type;
        progress = 0.03f;
    }

    public void addProgress(float delta) {
        progress = math.min(1f, math.max(progress + delta, 0f));
    }

    public void addHealProgress(float delta) {
        healProgress = math.min(1f, math.max(healProgress + delta, 0f));
    }

    public DiseaseStage getStage() {
        return type.getStage(progress);
    }
}
}