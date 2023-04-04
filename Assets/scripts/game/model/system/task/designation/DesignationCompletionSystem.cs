using game.model.component;
using game.model.component.task;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.TaskStatusEnum;

namespace game.model.system.task.designation {
    // deletes cancelled and completed designations
    // removes task from failed designations to be reopened later 
    public class DesignationCompletionSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<DesignationComponent, TaskFinishedComponent> filter;

        public override void Run() {
            foreach (var i in filter) {
                ref EcsEntity designation = ref filter.GetEntity(i);
                TaskFinishedComponent taskFinishedComponent = filter.Get2(i);
                Debug.Log("[DesignationCompletionSystem]: completing designation " + designation.take<DesignationComponent>().type.name + " " + designation.pos());

                detachTask(designation, taskFinishedComponent); // detach task and designation from each other

                // remove designation entity, failed will recreate task
                TaskStatusEnum status = taskFinishedComponent.status;
                if (status == CANCELED || status == COMPLETE) {
                    Debug.Log("[DesignationCompletionSystem]: deleting designation " + designation.take<DesignationComponent>().type.name + " " + designation.pos());
                    model.designationContainer.removeDesignation(designation);
                } else if (status == FAILED) {
                    designation.Replace(new TaskCreationTimeoutComponent { value = 50 });
                }
            }
        }

        private void detachTask(EcsEntity designation, TaskFinishedComponent taskFinishedComponent) {
            if (designation.Has<TaskComponent>()) {
                ref EcsEntity task = ref designation.takeRef<TaskComponent>().task;
                if (task.IsAlive()) {
                    task.Replace(taskFinishedComponent);
                    Debug.Log("[DesignationCompletionSystem]: deleting taskDesignation component from task");
                    task.Del<TaskDesignationComponent>();
                }
                Debug.Log("[DesignationCompletionSystem]: deleting task component from designation " + designation.take<DesignationComponent>().type.name + " " + designation.pos());
                designation.Del<TaskComponent>();
            }
        }
    }
}