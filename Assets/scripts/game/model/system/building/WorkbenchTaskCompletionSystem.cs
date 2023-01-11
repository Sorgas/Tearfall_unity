using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

// if WB marked with finished task, task and current order should be reset, then OrderSelectionSystem will select new Current order
namespace game.model.system.building {
    class WorkbenchTaskCompletionSystem : IEcsRunSystem {
        public EcsFilter<WorkbenchComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                if (validateEntity(entity)) {
                    handle(i, entity);
                }
            }
        }

        private void handle(int i, EcsEntity entity) {
            ref WorkbenchComponent workbench = ref filter.Get1(i);
            TaskFinishedComponent taskFinished = filter.Get2(i);
            CraftingOrder order = entity.take<WorkbenchCurrentOrderComponent>().currentOrder;
            Debug.Log("[WorkbenchTaskCompletionSystem] completing order " + order.name + " in WB " + entity.name() + " : " + taskFinished.status);
            entity.Del<WorkbenchCurrentOrderComponent>();
            switch (taskFinished.status) {
                case TaskStatusEnum.COMPLETE: {
                    order.performedQuantity++;
                    entity.Replace(new UiUpdateComponent());
                }
                    break;
                case TaskStatusEnum.FAILED: {
                    if (taskFinished.reason == "no_materials") {
                        Debug.Log("order " + order.name + " paused due to lack of materials.");
                        order.paused = true;
                        order.status = CraftingOrder.CraftingOrderStatus.PAUSED_PROBLEM;
                        entity.Replace(new UiUpdateComponent());
                    }
                }
                    break;
                case TaskStatusEnum.CANCELED: {
                    // recreate task
                }
                    break;
            }
            detachTask(entity, taskFinished);
            entity.Del<TaskFinishedComponent>();
        }

        private void detachTask(EcsEntity workbench, TaskFinishedComponent taskFinishedComponent) {
            if (workbench.Has<TaskComponent>()) {
                ref EcsEntity task = ref workbench.takeRef<TaskComponent>().task;
                if (task.IsAlive()) {
                    task.Replace(taskFinishedComponent);
                    Debug.Log("[WorkbenchTaskCompletionSystem]: deleting taskBuilding component from task");
                    task.Del<TaskBuildingComponent>();
                }
                Debug.Log("[WorkbenchTaskCompletionSystem]: deleting task component from workbench " + workbench.name());
                workbench.Del<TaskComponent>();
            }
        }

        // if WB is marked with finished task, it should have current order. Task reference is optional
        private bool validateEntity(EcsEntity entity) {
            if (!entity.Has<WorkbenchCurrentOrderComponent>()) {
                Debug.LogError("[WorkbenchTaskCompletionSystem]: workbench does not have current order component " + entity.name());
                return false;
            }
            return true;
        }
    }
}