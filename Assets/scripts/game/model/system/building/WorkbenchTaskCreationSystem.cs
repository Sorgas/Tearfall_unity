using game.model.component;
using game.model.component.building;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.item;
using game.model.component.task.order;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

// if workbench has current order, task should be created
namespace game.model.system.building {
    public class WorkbenchTaskCreationSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<WorkbenchCurrentOrderComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter;

        public override void Run() {
            foreach(int i in filter) {
                // TODO set job
                ref WorkbenchCurrentOrderComponent component = ref filter.Get1(i);
                ref EcsEntity entity = ref filter.GetEntity(i);
                WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
                Action action = new CraftItemAtWorkbenchAction(component.currentOrder, entity);
                component.currentOrder.status = CraftingOrder.CraftingOrderStatus.PERFORMING;
                
                EcsEntity task = model.taskContainer.generator.createTask(action, workbench.job, TaskPriorities.JOB, model.createEntity(), model);
                entity.Replace(new TaskComponent{task = task});
                task.Replace(new TaskBuildingComponent{building = entity});
                model.taskContainer.addOpenTask(task);
                Debug.Log("[WorkbenchTaskCreationSystem] crafting task " + action.name + " created in " + entity.name());
            }
        }

    }
}