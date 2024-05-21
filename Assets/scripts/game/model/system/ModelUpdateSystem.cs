using game.model.container;

namespace game.model.system {
    // all updates to model made from mouse tools and ui, should pass through this system.
    // This ensures they will not be concurrent with updates made from other systems.
    public class ModelUpdateEcsSystem : LocalModelTimeIndependentEcsSystem {
        
        public override void Run() {
            foreach (ModelUpdateEvent updateEvent in model.getUpdateEvents()) {
                updateEvent.action.Invoke(model);
            }
        }
    }
}