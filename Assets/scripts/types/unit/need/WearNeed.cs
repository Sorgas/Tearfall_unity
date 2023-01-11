using types.action;

namespace types.unit.need {
    public class WearNeed : Need {

        public WearNeed() : base() {}

        public override TaskPriorityEnum getPriority(float value) {
            return TaskPriorityEnum.HEALTH_NEEDS;
        }
        //
        // public override boolean isSatisfied(CanvasScaler.Unit unit) {
        //     throw new System.NotImplementedException();
        // }
        //
        // public override Task tryCreateTask(CanvasScaler.Unit unit) {
        //     throw new System.NotImplementedException();
        // }
        //
        // public override MoodEffect getMoodPenalty(CanvasScaler.Unit unit, NeedState state) {
        //     throw new System.NotImplementedException();
        // }
    }
}