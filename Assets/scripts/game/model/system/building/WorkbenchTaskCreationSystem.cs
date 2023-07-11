using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.building;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.item;
using game.model.component.task.order;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder.CraftingOrderStatus;

// creates task for workbench.
namespace game.model.system.building {
public class WorkbenchTaskCreationSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<WorkbenchComponent>.Exclude<TaskComponent> filter;
    private const bool debug = true;

    public override void Run() {
        foreach (int i in filter) {
            ref WorkbenchComponent component = ref filter.Get1(i);
            if (!component.hasActiveOrders) return;
            updateWithRolling(filter.GetEntity(i), ref component);
        }
    }

    // checks orders of workbench
    // moves completed and reopened orders to end of list
    // creates task for incomplete orders
    private void updateWithRolling(EcsEntity entity, ref WorkbenchComponent workbench) {
        CraftingOrder firstOrder = workbench.orders[0];
        BuildingComponent building = entity.take<BuildingComponent>();
        Vector3Int accessPosition = building.type.getAccessByPositionAndOrientation(entity.pos(), building.orientation);
        if (firstOrder.status == PAUSED) { // order paused, move to end
            if (workbench.orders.Count > 0) moveFirstOrderToEnd(ref workbench, entity);
        } else {
            if (firstOrder.performedQuantity < firstOrder.targetQuantity) { // incomplete order, try create task
                if (checkOrderTaskCreation(firstOrder, accessPosition)) {
                    createTaskForOrder(firstOrder, entity);
                } else { // pause order with missing items
                    firstOrder.status = PAUSED; // TODO add warning to player log
                }
            } else { // order completed, delete or move to end
                if (firstOrder.repeated) { // move repeated order to end
                    firstOrder.performedQuantity = 0;
                    moveFirstOrderToEnd(ref workbench, entity);
                } else { // delete not repeated order
                    workbench.orders. RemoveAt(0);
                    log("order removed " + firstOrder.name);
                }
                workbench.updated = true; // causes order list in ui to redraw
            }
            firstOrder.updated = true; // causes order line in ui to redraw
        }
        workbench.update();
    }

    // creates task for given order. links: task to wb, task to order, wb to task. 
    private void createTaskForOrder(CraftingOrder order, EcsEntity entity) {
        // TODO update ui (status icon change)
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        Action action = new CraftItemAtWorkbenchAction(order, entity);
        order.status = PERFORMING;
        EcsEntity task = model.taskContainer.generator.createTask(action, workbench.job, workbench.priority, model.createEntity(), model);
        entity.Replace(new TaskComponent { task = task }); // link wb to task
        task.Replace(new TaskCraftingOrderComponent { order = order }); // link task to order
        task.Replace(new TaskBuildingComponent { building = entity }); // link task to wb
        model.taskContainer.addOpenTask(task);
        log("crafting task " + action.name + " created in " + entity.name());
    }

    private void moveFirstOrderToEnd(ref WorkbenchComponent component, EcsEntity entity) {
        // TODO update ui (order line movement)
        CraftingOrder firstOrder = component.orders[0];
        component.orders.RemoveAt(0);
        component.orders.Add(firstOrder);
        component.orders[0].updated = true;
        firstOrder.status = WAITING;
        firstOrder.updated = true;
        component.updated = true;
        log("moving order to end " + firstOrder.name);
    }

    private bool checkOrderTaskCreation(CraftingOrder order, Vector3Int position) {
        bool itemsFound = model.itemContainer.craftingUtil.findItemsForOrder(order, position);
        order.ingredients.ForEach(ingredientOrder => ingredientOrder.items.Clear());
        return itemsFound;
    }
    
    // finds first order with enough items to create task and creates task
    private void updateWithoutRolling(EcsEntity entity, WorkbenchComponent component) {
        BuildingComponent building = entity.take<BuildingComponent>();
        Vector3Int accessPosition = building.type.getAccessByPositionAndOrientation(entity.pos(), building.orientation);
        List<CraftingOrder> activeOrders = component.orders
            .Where(order => order.status != PAUSED)
            .Where(order => order.targetQuantity == -1 || order.targetQuantity > order.performedQuantity)
            .ToList();
        foreach (var order in activeOrders) {
            if (checkOrderTaskCreation(order, accessPosition)) {
                createTaskForOrder(order, entity);
                break;
            }
        }
    }
    
    private void log(string message) {
        if (debug) Debug.Log("[WorkbenchTaskCreationSystem] " + message);
    }
}
}