using System.Collections.Generic;
using game.model;
using game.model.component;
using game.model.component.building;
using game.model.component.task;
using game.model.component.task.order;
using game.model.container;
using game.view.ui.util;
using game.view.util;
using generation.item;
using Leopotam.Ecs;
using TMPro;
using types.action;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.workbench {
public class WorkbenchWindowHandler : WindowManagerMenu {
    public const string name = "workbench";
    public TextMeshProUGUI workbenchNameText;
    public Button addOrderButton;
    public GameObject orderList;
    public TextMeshProUGUI noOrdersText;
    public WorkbenchRecipeListHandler recipeListHandler;

    public WorkbenchInventoryHandler inventory;
    public Button showInventoryButton;

    private CraftingOrderGenerator generator = new();
    private List<CraftingOrderLineHandler> orderLines = new();
    private EcsEntity entity; // workbench

    public void Start() {
        addOrderButton.onClick.AddListener(() => recipeListHandler.gameObject.SetActive(!recipeListHandler.gameObject.activeSelf));
        showInventoryButton.onClick.AddListener(() => inventory.toggle());
    }

    // fill wb with recipes and orders
    public void init(EcsEntity entity) {
        this.entity = entity;
        workbenchNameText.text = entity.name();
        refillOrderList(entity.take<WorkbenchComponent>());
        recipeListHandler.gameObject.SetActive(false);
        recipeListHandler.fillFor(entity);
        inventory.hide();
        inventory.initFor(entity);
    }

    // refills order list if orders was updated by wb system
    public void Update() {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        if (!workbench.updatedFromModel) return;
        Debug.Log("updating order list");
        refillOrderList(workbench);
        workbench.updatedFromModel = false;
    }

    // creates order to the end of the order list
    public void createOrder(string recipeName) {
        int index = orderLines.Count;
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        CraftingOrder order = generator.generate(recipeName);
        workbench.orders.Insert(index, order);
        workbench.hasActiveOrders = true;
        createOrderLine(index, order);
    }

    public void duplicateOrder(CraftingOrder order) {
        CraftingOrder newOrder = new CraftingOrder(order);
        newOrder.status = CraftingOrder.CraftingOrderStatus.WAITING;
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        int index = workbench.orders.IndexOf(order);
        workbench.orders.Insert(index + 1, newOrder);
        createOrderLine(index + 1, newOrder);
    }

    // creates ui line for order, inserts it at index. Moves all following order lines lower. Updates adjacent order lines
    private void createOrderLine(int index, CraftingOrder order) {
        GameObject line = PrefabLoader.create("craftingOrderLine", orderList.transform, new Vector3(0, (index) * -120, 0));
        line.GetComponent<CraftingOrderLineHandler>().init(order, entity, this);
        orderLines.Insert(index, line.GetComponent<CraftingOrderLineHandler>());
        for (var i = index + 1; i < orderLines.Count; i++) {
            moveOrderLine(orderLines[i].gameObject, false);
        }
        if (index - 1 >= 0) orderLines[index - 1].updateVisual();
        if (orderLines.Count > index + 1) orderLines[index + 1].updateVisual();
    }

    public void removeOrder(CraftingOrder order) => removeOrder(getOrderIndex(order));

    // removes order, cancels its task if present
    private void removeOrder(int index) {
        // remove order line
        // move other lines
        // remove order from workbench

        // if was current
        // cancel task of canceled order
        // remove current order componentfrom workbench

        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        Debug.Log(workbench.orders.Count);
        CraftingOrder order = workbench.orders[index];
        Debug.Log("[WorkbenchWindowHandler] removing order " + order.name + " " + index);
        Destroy(orderLines[index].gameObject);
        orderLines.RemoveAt(index);
        workbench.orders.RemoveAt(index);
        workbench.update();
        for (var i = index; i < orderLines.Count; i++) { // move other order lines up
            moveOrderLine(orderLines[i].gameObject, true);
        }
        // if current order is deleted, its task is removed
        if (entity.Has<TaskComponent>()) {
            EcsEntity task = entity.take<TaskComponent>().task;
            if (task.take<TaskCraftingOrderComponent>().order == order) {
                GameModel.get().currentLocalModel.addUpdateEvent(
                    new ModelUpdateEvent(model => model.taskContainer.removeTask(task, TaskStatusEnum.CANCELED)));
            }
        }
    }

    public void moveOrder(CraftingOrder order, bool up) {
        int orderIndex = getOrderIndex(order);
        if (orderIndex < 0) {
            Debug.LogError("[WorkbenchWindowHandler] order " + order.name + " not found in WB " + entity.name());
            return;
        }
        int index2 = orderIndex + (up ? -1 : 1);
        if (index2 >= 0 && index2 < orderLines.Count) {
            swapOrderLines(orderIndex, index2);
        }
    }

    private void refillOrderList(WorkbenchComponent workbench) {
        foreach (var line in orderLines) {
            Destroy(line.gameObject);
        }
        orderLines.Clear();
        for (var i = 0; i < workbench.orders.Count; i++) {
            createOrderLine(i, workbench.orders[i]);
        }
    }

    private int getOrderIndex(CraftingOrder order) {
        for (int i = 0; i < orderLines.Count; i++) {
            if (orderLines[i].order == order) {
                Debug.Log(i);
                return i;
            }
        }
        Debug.Log(-1);
        return -1;
    }

    private void swapOrderLines(int index1, int index2) {
        (orderLines[index1], orderLines[index2]) = (orderLines[index2], orderLines[index1]);
        (orderLines[index1].transform.localPosition, orderLines[index2].transform.localPosition) =
            (orderLines[index2].transform.localPosition, orderLines[index1].transform.localPosition);
        ref WorkbenchComponent component = ref entity.takeRef<WorkbenchComponent>();
        (component.orders[index1], component.orders[index2]) = (component.orders[index2], component.orders[index1]);
        orderLines[index1].updateVisual();
        orderLines[index2].updateVisual();
    }

    // moves order line up or down by one itemHeight
    private void moveOrderLine(GameObject obj, bool up) {
        Debug.Log("[WorkbenchWindowHandler] moving order line " + (up ? "up" : "down"));
        Vector3 localPosition = obj.transform.localPosition;
        localPosition.y += up ? 120 : -120;
        obj.transform.localPosition = localPosition;
    }

    public override string getName() => "workbench";
}
}