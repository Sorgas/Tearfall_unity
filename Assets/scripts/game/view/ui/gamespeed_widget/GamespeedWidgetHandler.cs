using game.model;
using game.view.ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// game can be paused and unpaused
// one of 3 speeds can be selected

// key presses handled through buttons
// buttons call handler methods
// handler methods update GameModel state and update buttons visual
public class GamespeedWidgetHandler : MonoBehaviour, IHotKeyAcceptor {
    public GameObject pauseButton;
    public GameObject speed1Button;
    public GameObject speed2Button;
    public GameObject speed3Button;
    public Color activeColor = new Color(0.75f, 0.75f, 0.75f, 1);
    public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1);

    public void Start() {
        pauseButton.GetComponent<Button>().onClick.AddListener(() => togglePause());
        speed1Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(1));
        speed2Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(2));
        speed3Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(3));
    }

    public void togglePause() {
        GameModel.get().updateController.paused = !GameModel.get().updateController.paused;
        updateVisual();
    }

    public void setSpeed(int speed) {
        GameModel.get().updateController.paused = false;
        GameModel.get().updateController.setSpeed(speed);
        updateVisual();
    }

    public bool accept(KeyCode key) {
        Debug.Log("speed widget: " + key);
        if (key == KeyCode.Space) {
            ExecuteEvents.Execute(pauseButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return true;
        }
        if (key == KeyCode.Keypad1 || key == KeyCode.Alpha1) {
            ExecuteEvents.Execute(speed1Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return true;
        }
        if (key == KeyCode.Keypad2 || key == KeyCode.Alpha2) {
            ExecuteEvents.Execute(speed2Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return true;
        }
        if (key == KeyCode.Keypad3 || key == KeyCode.Alpha3) {
            ExecuteEvents.Execute(speed3Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return true;
        }
        return false;
    }

    public void updateVisual() {
        deactivateAll();
        if (GameModel.get().updateController.paused) {
            pauseButton.GetComponent<Image>().color = activeColor;
        } else {
            switch (GameModel.get().updateController.speed) {
                case 1:
                    speed1Button.GetComponent<Image>().color = activeColor;
                    break;
                case 2:
                    speed2Button.GetComponent<Image>().color = activeColor;
                    break;
                case 3:
                    speed3Button.GetComponent<Image>().color = activeColor;
                    break;
            }
        }
    }

    private void deactivateAll() {
        pauseButton.GetComponent<Image>().color = inactiveColor;
        speed1Button.GetComponent<Image>().color = inactiveColor;
        speed2Button.GetComponent<Image>().color = inactiveColor;
        speed3Button.GetComponent<Image>().color = inactiveColor;
    }
}