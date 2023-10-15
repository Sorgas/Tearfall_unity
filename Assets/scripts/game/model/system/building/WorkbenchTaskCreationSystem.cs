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

namespace game.model.system.building {
// creates task for workbench. 
public class WorkbenchTaskCreationSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<WorkbenchComponent>.Exclude<TaskComponent> filter;

    public WorkbenchTaskCreationSystem() => debug = true;

    public override void Run() {
        foreach (int i in filter) {
            ref WorkbenchComponent component = ref filter.Get1(i);
            EcsEntity entity = filter.GetEntity(i);
            if (component.hasActiveOrders) {
                updateWithRolling(entity, ref component);
            }
        }
    }

    // checks first order of workbench 
    // can move order to the end of the list or create task for order
    // Update flag is always reset for first order. Update flag is reset for neighbouring orders when order moves.
    // Update flag is reset for workbench when order list content or order change
    private void updateWithRolling(EcsEntity entity, ref WorkbenchComponent workbench) {
        CraftingOrder order = workbench.orders[0];
        if (order.status == PAUSED) { // order paused, move to end
            if (workbench.orders.Count > 0) moveOrderToEnd(entity, order);
            log($"paused order {order.name} moved to end of the list");
        } else {
            if (order.performedQuantity < order.targetQuantity) { // incomplete order, try create task
                if (checkTaskCreation(order, entity)) {
                    createTaskForOrder(order, entity);
                    order.updated = true;
                } else { // pause order with missing items
                    order.status = PAUSED; // TODO add warning to player log
                    log($"order {order.name} paused: insufficient ingredient items");
                    order.updated = true;
                }
            } else {
                if (order.repeated) { // reset and move to end
                    order.performedQuantity = 0;
                    order.status = WAITING;
                    moveOrderToEnd(entity, order);
                    log($"completed repeated order {order.name} moved to end of the list");
                } else {
                    // TODO have option to pause completed non-repeated orders
                    workbench.orders.RemoveAt(0);
                    workbench.updatedFromModel = true; // causes order list in ui to redraw
                    log("order removed " + order.name);
                }
            }
        }
        order.updated = true; // causes order line in ui to redraw
        workbench.update();
    }

    private bool checkTaskCreation(CraftingOrder order, EcsEntity workbench) {
        BuildingComponent building = workbench.take<BuildingComponent>();
        Vector3Int accessPosition = building.type.getAccessByPositionAndOrientation(workbench.pos(), building.orientation);
        return model.itemContainer.craftingUtil.checkItemsForOrder(order, accessPosition);
    }

    // creates task for given order. links: task to wb, task to order, wb to task. 
    private void createTaskForOrder(CraftingOrder order, EcsEntity entity) {
        // TODO update ui (status icon change)
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        Action action = new CraftItemAtWorkbenchAction(order, entity);
        order.status = PERFORMING;
        int priority = entity.take<PriorityComponent>().priority;
        EcsEntity task = model.taskContainer.generator.createTask(action, workbench.job, priority, model.createEntity(), model);
        entity.Replace(new TaskComponent { task = task }); // link wb to task
        task.Replace(new TaskCraftingOrderComponent { order = order }); // link task to order
        task.Replace(new TaskBuildingComponent { building = entity }); // link task to wb
        model.taskContainer.addOpenTask(task);
        log($"task { action.name} for order {order.name} created in {entity.name()}");
    }

    private void moveOrderToEnd(EcsEntity workbench, CraftingOrder order) {
        WorkbenchComponent component = workbench.take<WorkbenchComponent>();
        int index = component.orders.IndexOf(order);
        component.orders.RemoveAt(index);
        component.orders.Add(order);
        component.orders[0].updated = true;
        order.updated = true;
        component.updatedFromModel = true;
    }

    private bool checkOrderTaskCreation(CraftingOrder order, Vector3Int position) {
        return model.itemContainer.craftingUtil.checkItemsForOrder(order, position);
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