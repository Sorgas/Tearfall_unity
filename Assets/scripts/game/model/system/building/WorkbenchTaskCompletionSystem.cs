using enums.action;
using game.model.component;
using game.model.component.building;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

// if WB marked with finished task, task and current order should be reset, then OrderSelectionSystem will select new Current order
class WorkbenchTaskCompletionSystem : IEcsRunSystem {
    public EcsFilter<WorkbenchComponent, WorkbenchCurrentOrderComponent, TaskFinishedComponent> filter;
    
    public void Run() {
        foreach(int i in filter) {
            EcsEntity entity = filter.GetEntity(i);
            ref WorkbenchComponent workbench = ref filter.Get1(i);
            WorkbenchCurrentOrderComponent currentOrderComponent = filter.Get2(i);
            TaskFinishedComponent taskFinished = filter.Get3(i);
            CraftingOrder order = currentOrderComponent.currentOrder;

            Debug.Log("[WorkbenchTaskCompletionSystem] completing order " + order.name + " in WB " + entity.name() + " : " + taskFinished.status);
            entity.Del<WorkbenchCurrentOrderComponent>();
            entity.Del<TaskComponent>();
            switch(taskFinished.status) {
                case TaskStatusEnum.COMPLETE: {
                    order.performedQuantity++;
                    entity.Replace(new UiUpdateComponent());
                }
                break;
                case TaskStatusEnum.FAILED: {
                    if(taskFinished.reason == "no_materials") {
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
            entity.Del<TaskFinishedComponent>();
        }
    }
}