using enums.action;
using game.model.component;
using game.model.component.task;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.DesignationComponents;

namespace game.model.system.task.designation {

    // deletes cancelled and completed designations
    // removes task from failed designations to be reopened later 
    public class DesignationCompletionSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                ref EcsEntity designation = ref filter.GetEntity(i);
                TaskFinishedComponent taskFinishedComponent = filter.Get2(i);
                Debug.Log("DesignationCompletionSystem: completing designation " + designation.Get<DesignationComponent>().type.NAME);

                // detach task
                detachTask(designation, taskFinishedComponent);
                
                // remove designation entity, failed will recreate task
                TaskStatusEnum status = taskFinishedComponent.status;
                if (status == TaskStatusEnum.CANCELED || status == TaskStatusEnum.COMPLETE) {
                    GameModel.get().designationContainer.removeDesignation(designation);
                }
                designation.Del<TaskFinishedComponent>();
            }
        }

        private void detachTask(EcsEntity designation, TaskFinishedComponent taskFinishedComponent) {
            if (designation.Has<TaskComponent>()) {
                ref EcsEntity task = ref designation.Get<TaskComponent>().task;
                task.Replace(taskFinishedComponent);
                task.Del<TaskComponents.TaskDesignationComponent>();
                designation.Del<TaskComponent>();
            }
        }
    }
}