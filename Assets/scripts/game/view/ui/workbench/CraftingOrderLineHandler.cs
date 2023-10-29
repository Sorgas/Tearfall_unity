using System.Collections.Generic;
using game.model;
using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using game.model.container;
using game.view.ui.tooltip.handler;
using game.view.ui.tooltip.trigger;
using Leopotam.Ecs;
using TMPro;
using types.action;
using types.item.type;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder.CraftingOrderStatus;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.workbench {
public class CraftingOrderLineHandler : MonoBehaviour {
    public SquareIconButtonHandler pauseButton;
    public SquareIconButtonHandler repeatButton;
    public SquareIconButtonHandler configureButton;
    public SquareIconButtonHandler upButton;
    public SquareIconButtonHandler downButton;
    public SquareIconButtonHandler cancelButton;
    public SquareIconButtonHandler duplicateButton;

    public TextMeshProUGUI text;
    public Image statusIcon; // TODO use this instead of statusText
    public TextMeshProUGUI statusText;
    public Image itemImage;
    public CraftingOrderConfigPanelHandler configurePanel;

    // quantity
    public SquareIconButtonHandler plusButton;
    public SquareIconButtonHandler minusButton;
    public TMP_InputField quantityInputField;

    private EcsEntity workbench;
    public WorkbenchWindowHandler workbenchWindow;
    public CraftingOrder order;
    // private CraftingOrder order;

    public void Start() {
        pauseButton.addListener(togglePaused);
        repeatButton.addListener(toggleRepeated);
        upButton.addListener(() => workbenchWindow.moveOrder(order, true));
        downButton.addListener(() => workbenchWindow.moveOrder(order, false));
        cancelButton.addListener(() => workbenchWindow.removeOrder(order));
        duplicateButton.addListener(() => workbenchWindow.duplicateOrder(order));
        plusButton.addListener(() => changeQuantity(1));
        minusButton.addListener(() => changeQuantity(-1));
        ReusedTooltipClickTrigger trigger = configureButton.GetComponent<ReusedTooltipClickTrigger>();
        trigger.tooltip = configurePanel.GetComponent<HidingTooltipHandler>();
        trigger.closeCallback = () => configureButton.setColor(BUTTON_NORMAL);
        trigger.openCallback = () => {
            configurePanel.fillFor(order);
            configureButton.setColor(BUTTON_CHOSEN);
        };
    }

    // order can be updated from game model, changes should be visible in ui
    public void Update() {
        if (!order.updated) return;
        Debug.Log("updating order line visual");
        updateVisual();
        order.updated = false;
    }

    public void init(CraftingOrder order, EcsEntity workbench, WorkbenchWindowHandler window) {
        this.order = order;
        workbenchWindow = window;
        this.workbench = workbench;
        text.text = order.name;
        itemImage.sprite = ItemTypeMap.get().getSprite(order.recipe.newType);
        configurePanel = window.orderConfigPanel;
        updateVisual();
    }

    public void updateVisual() {
        updateQuantityVisual();
        updateStatusText();
        updatePausedVisual();
        updateMoveArrows();
    }

    private void toggleRepeated() {
        order.repeated = !order.repeated;
        repeatButton.GetComponent<SquareIconButtonHandler>().setColor(order.repeated ? BUTTON_CHOSEN : BUTTON_NORMAL);
    }

    private void togglePaused() {
        order.status = order.status == PAUSED ? WAITING : PAUSED;
        pauseButton.GetComponent<SquareIconButtonHandler>().setColor(order.status == PAUSED ? BUTTON_NORMAL : BUTTON_CHOSEN);
        Debug.Log("pausing order");
        if (order.status == PAUSED) {
            if (workbench.Has<TaskComponent>()) {
                Debug.Log("has task");
                GameModel.get().currentLocalModel.addUpdateEvent(new ModelUpdateEvent(model => {
                    model.taskContainer.removeTask(workbench.take<TaskComponent>().task, TaskStatusEnum.CANCELED);
                }));
            }
        } else {
            workbench.takeRef<WorkbenchComponent>().hasActiveOrders = true;
        }
    }

    // order modification should not interfere with order handling in WorkbenchTaskCreationSystem
    private void changeQuantity(int delta) {
        GameModel.get().currentLocalModel.addUpdateEvent(new ModelUpdateEvent(model => {
            order.targetQuantity += delta;
            updateQuantityVisual();
        }));
    }

    private void updateQuantityVisual() {
        quantityInputField.text = order.performedQuantity + "/" + order.targetQuantity;
        minusButton.gameObject.SetActive(order.targetQuantity > 1);
        plusButton.gameObject.SetActive(order.targetQuantity < 100);
    }

    private void updateStatusText() {
        statusText.text = selectTextForStatus();
    }

    private void updatePausedVisual() {
        pauseButton.setColor(order.status == PAUSED ? BUTTON_CHOSEN : BUTTON_NORMAL);
        gameObject.GetComponent<Image>().color = order.status == PAUSED ? background : backgroundHighlight;
    }

    private void updateMoveArrows() {
        List<CraftingOrder> orders = workbench.take<WorkbenchComponent>().orders;
        int currentIndex = orders.IndexOf(order);
        upButton.gameObject.SetActive(currentIndex > 0);
        downButton.gameObject.SetActive(currentIndex < orders.Count - 1);
    }
    
    private string selectTextForStatus() {
        return order.status switch {
            PERFORMING => "A", // means 'active'
            WAITING => "W",
            PAUSED => "P",
            // CraftingOrderStatus.PAUSED_PROBLEM => "PP",
            _ => "E"
        };
    }
}
}