using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.unit.skill;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.plant {
    public class PlantRemoveAction : Action {
        private EcsEntity entity;
        public PlantRemoveAction(EcsEntity plant) : base(new PlantActionTarget(plant)) {
            entity = plant;
            usedSkill = UnitSkills.FARMING.name;

            startCondition = () => {
                if (!plant.IsAlive()) return OK;
                // TODO tool?
                return OK;
            };
            
            
            onStart = () => {
                maxProgress = 50;
                speed = getSpeed();
            };

            onFinish = () => {
                model.plantContainer.removePlant(entity);
            };
        }
    }
}