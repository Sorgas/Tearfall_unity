using game.model.component.task.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using static types.action.ActionCheckingEnum;

namespace game.model.action.plant {
    public class PlantRemoveAction : Action {
        private EcsEntity entity;
        public PlantRemoveAction(EcsEntity plant) : base(new EntityActionTarget(plant, ActionTargetTypeEnum.ANY)) {
            entity = plant;
            maxProgress = 50;
            startCondition = () => {
                if (!plant.IsAlive()) return OK;
                // TODO tool?
                return OK;
            };

            onFinish = () => {
                model.plantContainer.removePlant(entity);
            };
        }
    }
}