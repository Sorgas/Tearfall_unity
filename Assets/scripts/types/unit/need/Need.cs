using game.model.component.task.action;
using game.model.localmap;
using game.model.system.unit;
using Leopotam.Ecs;
using types.unit.disease;

namespace types.unit.need {
    // defines need thresholds, priorities, penalties, task creation
    // need is value from 1 to 0, 1 means satisfied, 0 means 
    public abstract class Need {
        // public readonly string moodEffectKey;
        // public DiseaseType disease; TODO

        // returns priority by need value
        public abstract int getPriority(float value);

        // returns name of status effect to applied on given need value
        public abstract string getStatusEffect(float value);

        public abstract Action tryCreateAction(LocalModel model, EcsEntity unit);

        public abstract UnitTaskAssignment createDescriptor(LocalModel model, EcsEntity unit);

        public abstract UnitDisease createDisease();

        // public abstract int getHoursTo0();

        // public abstract TaskPriorityEnum countPriority(object component);

        // public abstract bool isSatisfied(CanvasScaler.Unit unit);

        // public abstract MoodEffect getMoodPenalty(CanvasScaler.Unit unit, NeedState state);

        // public float needLevel(CanvasScaler.Unit unit) {
        //     return unit.get(NeedAspect.class).needs.get(need).getRelativeValue();
        // }

        // public float diseaseLevel(CanvasScaler.Unit unit) {
        //     return disease != null 
        //         ? unit.get(BodyAspect.class).getDiseaseProgress(disease.name)
        //         : 0;
        // }
    }
}