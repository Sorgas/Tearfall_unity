using game.model.component;
using game.model.component.building;
using game.model.component.task.action;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

// if workbench has current order, task should be created
public class WorkbenchTaskCreationSystem : LocalModelEcsSystem {
    public EcsFilter<WorkbenchCurrentOrderComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter;

    public WorkbenchTaskCreationSystem(LocalModel model) : base(model) {}

    public override void Run() {
        foreach(int i in filter) {
            // TODO set job
            ref WorkbenchCurrentOrderComponent component = ref filter.Get1(i);       
            ref EcsEntity entity = ref filter.GetEntity(i);
            Action action = new CraftItemAtWorkbenchAction(component.currentOrder, entity);
            component.currentOrder.status = CraftingOrder.CraftingOrderStatus.PERFORMING;
            EcsEntity task = model.taskContainer.generator.createTask(action, model.createEntity(), model);
            entity.Replace(new TaskComponent{task = task});
            task.Replace(new TaskBuildingComponent{building = entity});
            model.taskContainer.addOpenTask(task);
            Debug.Log("[WorkbenchTaskCreationSystem] crafting task " + action.name + " created in " + entity.name());
        }
    }
}