namespace game.model.system {
    public class ModelUpdateSystem : LocalModelScalableEcsSystem {

        protected override void runLogic(int ticks) {
            foreach (string updateEvent in model.getUpdateEvents()) {
                
            }
        }
    }
}