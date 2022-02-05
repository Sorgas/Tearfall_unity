using enums.action;
using game.model.component;
using Leopotam.Ecs;
using static game.model.component.task.DesignationComponents;

namespace game.model.system.task.designation {

    // deletes cancelled and completed designations
    // removes task from failed designations to be reopened later 
    public class DesignationCompletionSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                EcsEntity designation = filter.GetEntity(i);
                TaskFinishedComponent taskFinishedComponent = filter.Get2(i);
                // detach task
                if (designation.Has<TaskComponent>()) {
                    designation.Get<TaskComponent>().task.Replace(taskFinishedComponent);
                    designation.Del<TaskComponent>();
                }
                designation.Del<TaskFinishedComponent>();
                // remove designation entity, failed will recreate task
                if (taskFinishedComponent.status == TaskStatusEnum.CANCELED || taskFinishedComponent.status == TaskStatusEnum.COMPLETE) {
                    GameModel.get().designationContainer.removeDesignation(designation);
                }
            }
        }
    }
}