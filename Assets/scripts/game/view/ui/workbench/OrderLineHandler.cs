using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderLineHandler : MonoBehaviour {
    public Button pauseButton;
    public Button repeatButton;
    public Button configureButton;
    public Button upButton;
    public Button downButton;
    public Button cancelButton;
    public Button duplicateButton;

    public Image statusIcon;
    public Image itemImage;

    public Button plusButton;
    public Button minusButton;
    public TMP_InputField quantityInputField;

    private EcsEntity workbench;
    private WorkbenchWindowHandler workbenchWindow;
    // private CraftingOrder order;

    public void Start() {
        pauseButton.onClick.AddListener(() => togglePaused());
        repeatButton.onClick.AddListener(() => toggleRepeated());
        configureButton.onClick.AddListener(() => showConfigureMenu());
        upButton.onClick.AddListener(() => move(true));
        downButton.onClick.AddListener(() => move(false));
        cancelButton.onClick.AddListener(() => cancel());
        duplicateButton.onClick.AddListener(() => copy());
        plusButton.onClick.AddListener(() => changeQuantity(1));
        minusButton.onClick.AddListener(() => changeQuantity(-1));
    }

    public void initForOrder(CraftingOrder order) {
        
    }

    public void toggleRepeated() {
        // order
    }

    public void togglePaused() {

    }

    public void showConfigureMenu() {

    }

    public void move(bool up) {

    }

    public void copy() {

    }

    public void cancel() {

    }

    private void changeQuantity(int delta) {

    }
}