using game.view.ui.util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using util.lang;

// game can be paused and unpaused
// one of 3 speeds can be selected

// key presses handled through buttons
// buttons call handler methods
// handler methods update GameModel state and update buttons visual
namespace game.view.ui.gamespeed_widget {
    public class GamespeedWidgetHandler : GameWidget {
        public GameObject pauseButton;
        public GameObject speed1Button;
        public GameObject speed2Button;
        public GameObject speed3Button;
        public Color activeColor = new(0.75f, 0.75f, 0.75f, 1);
        public Color inactiveColor = new(0.4f, 0.4f, 0.4f, 1);
        private float previousSpeed;
        
        public void Start() {
            pauseButton.GetComponent<Button>().onClick.AddListener(togglePause);
            speed1Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(1));
            speed2Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(3));
            speed3Button.GetComponent<Button>().onClick.AddListener(() => setSpeed(6));
        }
        
        public override void init() => updateVisual();

        private void togglePause() {
            if (Time.timeScale != 0) {
                previousSpeed = Time.timeScale;
                Time.timeScale = 0;
            } else {
                Time.timeScale = previousSpeed;
            }
            updateVisual();
        }

        private void setSpeed(int speed) {
            Time.timeScale = speed;
            updateVisual();
        }

        public override bool accept(KeyCode key) {
            if (key == KeyCode.Space) return pressButton(pauseButton);
            if (key == KeyCode.Keypad1 || key == KeyCode.Alpha1) return pressButton(speed1Button);
            if (key == KeyCode.Keypad2 || key == KeyCode.Alpha2) return pressButton(speed2Button);
            if (key == KeyCode.Keypad3 || key == KeyCode.Alpha3) return pressButton(speed3Button);
            return false;
        }

        public override string getName() {
            return "game_speed_widget";
        }

        public override void reset() { }
        
        private bool pressButton(GameObject button) {
            ExecuteEvents.Execute(button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return true;
        }
        
        private void updateVisual() {
            pauseButton.GetComponent<Image>().color = Time.timeScale == 0 ? activeColor : inactiveColor;
            speed1Button.GetComponent<Image>().color = Time.timeScale == 1 ? activeColor : inactiveColor;
            speed2Button.GetComponent<Image>().color = Time.timeScale == 3 ? activeColor : inactiveColor;
            speed3Button.GetComponent<Image>().color = Time.timeScale == 6 ? activeColor : inactiveColor;
        }
    }
}