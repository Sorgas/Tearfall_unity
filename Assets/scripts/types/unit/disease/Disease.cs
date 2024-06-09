using System.Collections.Generic;
using System.Linq;
using game.model.system.unit;

namespace types.unit.disease {
// Diseases are special conditions for units.
public abstract class Disease {
    public readonly string name;
    public bool lethal;
    public float hoursToKill;
    public float progressDelta;
    public HashSet<DiseaseStage> stages = new();
    public List<string> treatments = new();
    public string description;
    public string notificationText;
    public string notificationIcon;

    protected Disease(string name, bool lethal, float hoursToKill, string description, string notificationText, string notificationIcon) {
        this.name = name;
        this.lethal = lethal;
        this.hoursToKill = hoursToKill;
        progressDelta = UnitDiseaseSystem.delta / hoursToKill;
        this.description = description;
        this.notificationIcon = notificationIcon;
        this.notificationText = notificationText;
    }

    public DiseaseStage getStage(float progress) {
        return stages.FirstOrDefault(stage => stage.maxProgress > progress && stage.minProgress <= progress);
    }
}

public class DiseaseStage {
    public readonly string name;
    public readonly float minProgress;
    public readonly float maxProgress;
    public string effect;

    public DiseaseStage(string name, float minProgress, float maxProgress, string effect) {
        this.name = name;
        this.minProgress = minProgress;
        this.maxProgress = maxProgress;
        this.effect = effect;
    }
}
}