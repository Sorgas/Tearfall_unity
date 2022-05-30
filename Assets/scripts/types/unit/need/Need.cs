using enums.action;
using Leopotam.Ecs;
using UnityEngine.UI;

namespace enums.unit.need {
    // defines need thresholds, priorities, penalties, task creation
    public abstract class Need {
        // public readonly string moodEffectKey;
        // public DiseaseType disease; TODO

        public Need() {}

        public abstract TaskPriorityEnum countPriority(object component);
        
        // public abstract boolean isSatisfied(CanvasScaler.Unit unit);
        //
        // public abstract Task tryCreateTask(CanvasScaler.Unit unit);
        //
        // public abstract MoodEffect getMoodPenalty(CanvasScaler.Unit unit, NeedState state);
        //
        // public float needLevel(CanvasScaler.Unit unit) {
        //     return unit.get(NeedAspect.class).needs.get(need).getRelativeValue();
        // }
        //
        // public float diseaseLevel(CanvasScaler.Unit unit) {
        //     return disease != null 
        //         ? unit.get(BodyAspect.class).getDiseaseProgress(disease.name)
        //         : 0;
        // }
    }
}