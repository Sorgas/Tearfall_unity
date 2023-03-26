using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;

namespace game.model.component.task.action.zone {
    // action performed on zone. can lock tile of zone (see ZoneTrackingComponent)
    public class ZoneAction : Action {
        protected EcsEntity zone;
        
        public ZoneAction(EcsEntity zone, ActionTargetTypeEnum type) : base(new ZoneActionTarget(zone, type)) {
            
        }
    }
}