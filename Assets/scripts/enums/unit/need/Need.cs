using Leopotam.Ecs;
using UnityEngine.UI;

namespace enums.unit.need {
    public abstract class Need {
        public readonly string moodEffectKey;
        public NeedEnum need;
        // public DiseaseType disease; TODO

        public Need(string moodEffectKey) {
            this.moodEffectKey = moodEffectKey;
        }

        // public abstract TaskPriorityEnum countPriority(EcsEntity unit);
        //
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