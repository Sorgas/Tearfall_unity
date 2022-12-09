using System.Collections.Generic;
using Assets.scripts.types.item.recipe;
using enums.action;
using game.model.component;
using game.model.component.building;
using game.view.ui;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

public class WorkbenchWindowHandler : MbWindow, IHotKeyAcceptor {
    public const string name = "workbench";
    public TextMeshProUGUI workbenchNameText;
    public Button addOrderButton;
    public GameObject orderList;
    public TextMeshProUGUI noOrdersText;
    public WorkbenchRecipeListHandler recipeListHandler;

    public WorkbenchInventoryHandler inventory;
    public Button showInventoryButton;

    private List<OrderLineHandler> orderLines = new();
    private CraftingOrderGenerator generator = new();
    public EcsEntity entity;

    public void Start() {
        addOrderButton.onClick.AddListener(() => recipeListHandler.gameObject.SetActive(!recipeListHandler.gameObject.activeSelf));
        showInventoryButton.onClick.AddListener(() => inventory.toggle());
    }

    // fill wb with recipes and orders
    public void init(EcsEntity entity) {
        this.entity = entity;
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        workbenchNameText.text = entity.take<BuildingComponent>().type.name;
        fillOrdersList(workbench);
        recipeListHandler.gameObject.SetActive(false);
        recipeListHandler.fillFor(entity);
        inventory.hide();
        inventory.initFor(entity);
    }

    // refills order list 
    public void updateState() {
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        fillOrdersList(workbench);
    }

    public bool accept(KeyCode key) {
        if (key == KeyCode.Q) {
            WindowManager.get().closeWindow(name);
        }
        return true;
    }

    public void createOrder(string recipeName) => createOrder(recipeName, orderLines.Count);

    public void createOrder(string recipeName, int index) {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        Debug.Log("[WorkbenchWindowHandler] creating order " + recipeName + " " + index);
        Recipe recipe = RecipeMap.get().get(recipeName);
        CraftingOrder order = generator.generate(recipe);
        workbench.orders.Insert(index, order);
        workbench.hasActiveOrders = true;
        createOrderLine(index, order);
    }

    // creates ui line for order, inserts it at index. moves all following order lines lower.
    public void createOrderLine(int index, CraftingOrder order) {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        GameObject line = PrefabLoader.create("craftingOrderLine", orderList.transform, new Vector3(0, (index) * -120, 0));
        line.GetComponent<OrderLineHandler>().init(order, this);
        orderLines.Insert(index, line.GetComponent<OrderLineHandler>());
        for (var i = index + 1; i < orderLines.Count; i++) {
            moveOrderLine(orderLines[i].gameObject, false);
        }
    }

    public void removeOrder(CraftingOrder order) => removeOrder(getOrderIndex(order));

    public void removeOrder(int index) {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        CraftingOrder order = workbench.orders[index];
        Debug.Log("[WorkbenchWindowHandler] removing order " + order.name + " " + index);
        GameObject.Destroy(orderLines[index].gameObject);
        orderLines.RemoveAt(index);
        workbench.orders.RemoveAt(index);
        workbench.updateFlag();
        for (var i = index; i < orderLines.Count; i++) {
            moveOrderLine(orderLines[i].gameObject, true);
        }
        // if current order is deleted, WB marked with finished task
        if (entity.Has<TaskComponent>() && entity.Has<WorkbenchCurrentOrderComponent>()
            && entity.take<WorkbenchCurrentOrderComponent>().currentOrder == order) {
            entity.Replace(new TaskFinishedComponent { status = TaskStatusEnum.CANCELED });
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

    private void fillOrdersList(WorkbenchComponent workbench) {
        orderLines.Clear();
        foreach (Transform child in orderList.transform) {
            if (child.gameObject != noOrdersText.gameObject) {
                GameObject.Destroy(child.gameObject);
            }
        }
        for (var i = 0; i < workbench.orders.Count; i++) {
            createOrderLine(i, workbench.orders[i]);
        }
    }

    private int getOrderIndex(CraftingOrder order) {
        for (int i = 0; i < orderLines.Count; i++) {
            if (orderLines[i].order == order) return i;
        }
        return -1;
    }

    private void swapOrderLines(int index1, int index2) {
        float order1Y = orderLines[index1].transform.localPosition.y;
        float order2Y = orderLines[index2].transform.localPosition.y;
        OrderLineHandler buffer = orderLines[index1];
        orderLines[index1] = orderLines[index2];
        orderLines[index2] = buffer;
        orderLines[index1].transform.localPosition = new Vector3(0, order1Y, 0);
        orderLines[index2].transform.localPosition = new Vector3(0, order2Y, 0);
        ref WorkbenchComponent component = ref entity.takeRef<WorkbenchComponent>();
        CraftingOrder orderBuffer = component.orders[index1];
        component.orders[index1] = component.orders[index2];
        component.orders[index2] = orderBuffer;
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