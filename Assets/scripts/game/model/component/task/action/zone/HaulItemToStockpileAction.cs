using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;

namespace game.model.component.task.action.zone {
    // TODO add containers usage
    // when assigned, searches item that can be brought to stockpile. 
    public class HaulItemToStockpileAction : Action {
        private EcsEntity item;
        private EcsEntity stockpile;
        
        // TODO use zone as target
        public HaulItemToStockpileAction(ActionTarget target) : base(target) {
            startCondition = () => {
                return ActionConditionStatusEnum.FAIL;
            };

            onFinish = () => { };
        }

        private void check() {
            if (item == EcsEntity.Null) {
                // model.itemContainer.util.findForStockpile();
                // find item in zone area or fail
            }
            // find free cell in zone or fail
        }
    }
}