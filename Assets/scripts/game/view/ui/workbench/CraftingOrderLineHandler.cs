using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using TMPro;
using types.item.type;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder;

namespace game.view.ui.workbench {
public class CraftingOrderLineHandler : MonoBehaviour {
    public Button pauseButton;
    public Button repeatButton;
    public Button configureButton;
    public Button upButton;
    public Button downButton;
    public Button cancelButton;
    public Button duplicateButton;

    public TextMeshProUGUI text;
    public Image statusIcon;
    public TextMeshProUGUI statusText;
    public Image itemImage;
    public CraftingOrderConfigPanelHandler configurePanel;

    // quantity
    public Button plusButton;
    public Button minusButton;
    public TMP_InputField quantityInputField;

    private EcsEntity workbench;
    public WorkbenchWindowHandler workbenchWindow;
    public CraftingOrder order;
    // private CraftingOrder order;

    public void Start() {
        pauseButton.onClick.AddListener(() => togglePaused());
        repeatButton.onClick.AddListener(() => toggleRepeated());
        configureButton.onClick.AddListener(() => {
            if (configurePanel.gameObject.activeSelf) {
                closeConfigureMenu();
            } else {
                showConfigureMenu();
            }
        });
        upButton.onClick.AddListener(() => move(true));
        downButton.onClick.AddListener(() => move(false));
        cancelButton.onClick.AddListener(() => cancel());
        duplicateButton.onClick.AddListener(() => copy());
        plusButton.onClick.AddListener(() => changeQuantity(1));
        minusButton.onClick.AddListener(() => changeQuantity(-1));
        closeConfigureMenu();
    }

    public void init(CraftingOrder order, WorkbenchWindowHandler window) {
        this.order = order;
        workbenchWindow = window;
        text.text = order.name;
        quantityInputField.text = order.targetQuantity.ToString();
        statusText.text = selectTextForStatus();
        itemImage.sprite = ItemTypeMap.get().getSprite(order.recipe.newType);
        // TODO
    }

    public void toggleRepeated() {
        order.repeated = !order.repeated;
        // TODO update view
    }

    public void togglePaused() {
        order.paused = !order.paused;
        if (!order.paused) workbench.takeRef<WorkbenchComponent>().hasActiveOrders = true;
        ;
        // TODO update view
    }

    public void showConfigureMenu() {
        configurePanel.gameObject.SetActive(true);
        configurePanel.fillFor(order);
        configureButton.GetComponent<Image>().color = UiColorsEnum.BUTTON_CHOSEN;
    }

    public void closeConfigureMenu() {
        configurePanel.gameObject.SetActive(false);
        configureButton.GetComponent<Image>().color = UiColorsEnum.BUTTON_NORMAL;
    }

    public void move(bool up) {
        workbenchWindow.moveOrder(order, up);
    }

    public void copy() { }

    public void cancel() {
        workbenchWindow.removeOrder(order);
    }

    private void changeQuantity(int delta) { }

    private string selectTextForStatus() {
        switch (order.status) {
            case CraftingOrderStatus.PERFORMING: {
                return "A";
            }
            case CraftingOrderStatus.WAITING: {
                return "W";
            }
            case CraftingOrderStatus.PAUSED: {
                return "P";
            }
            case CraftingOrderStatus.PAUSED_PROBLEM: {
                return "PP";
            }
        }
        return "E";
    }
}
}