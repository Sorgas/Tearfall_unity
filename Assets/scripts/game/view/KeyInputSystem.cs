using System.Collections.Generic;
using game.view.ui;
using UnityEngine;
using util.lang;

namespace game.view {

    // dispatches all key presses in the game
    public class KeyInputSystem : Singleton<KeyInputSystem> {
        private List<KeyCode> keyCodes = new List<KeyCode>(); // TODO add all key except view scrolls
        private WindowManager windowManager = WindowManager.get();
        private WidgetManager widgetManager = WidgetManager.get();
        private List<KeyCode> pressedKeys = new List<KeyCode>();

        public void update() {
            pressedKeys.Clear();
            foreach (var key in keyCodes) {
                if (Input.GetKeyDown(key)) {
                    pressedKeys.Add(key);
                }
            }
            bool handledInWindow = false;
            foreach (var key in pressedKeys) {
                if (windowManager.accept(key)) handledInWindow = true;
            }
            if (!handledInWindow) {
                foreach (var key in pressedKeys) {
                    widgetManager.accept(key);
                }   
            }
        }
    }
}