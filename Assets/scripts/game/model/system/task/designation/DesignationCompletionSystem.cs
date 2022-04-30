using enums.action;
using game.model.component;
using game.model.component.task;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
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
                Debug.Log("[DesignationCompletionSystem]: completing designation " + designation.take<DesignationComponent>().type.name + " " + designation.pos());

                detachTask(designation, taskFinishedComponent); // detach task and designation from each other

                // remove designation entity, failed will recreate task
                TaskStatusEnum status = taskFinishedComponent.status;
                if (status == TaskStatusEnum.CANCELED || status == TaskStatusEnum.COMPLETE) {
                    Debug.Log("[DesignationCompletionSystem]: deleting designation " + designation.take<DesignationComponent>().type.name + " " + designation.pos());
                    GameModel.get().designationContainer.removeDesignation(designation);
                }
            }
        }

        private void detachTask(EcsEntity designation, TaskFinishedComponent taskFinishedComponent) {
            if (designation.Has<TaskComponent>()) {
                ref EcsEntity task = ref designation.takeRef<TaskComponent>().task;
                if (task.IsAlive()) {
                    task.Replace(taskFinishedComponent);
                    task.Del<TaskComponents.TaskDesignationComponent>();
                }
                designation.Del<TaskComponent>();
            }
        }
    }
}