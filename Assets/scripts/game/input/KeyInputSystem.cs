using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.input {
    // dispatches all key presses in the game
    // key event may go to ui-windows or ui-widgets. 
    public class KeyInputSystem : Singleton<KeyInputSystem> {
        private List<KeyCode> keyCodes;
        public readonly WindowManager windowManager = WindowManager.get();
        public readonly WidgetManager widgetManager = WidgetManager.get();
        private List<KeyCode> pressedKeys = new();
        public PlayerControls playerControls;
        
        public KeyInputSystem() {
            // WASDRF is handled by camera input system, because these keys should only move camera
            KeyCode[] keys = {
                KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6,KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9,KeyCode.Alpha0,
                KeyCode.Q, KeyCode.E, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
                KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
                KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M,
                KeyCode.Space
            };
            keyCodes = new List<KeyCode>(keys);
        }

        public void update() {
            collectPressedKeys();
            handlePressedKeys();
        }

        private void collectPressedKeys() {
            pressedKeys.Clear();
            foreach (var key in keyCodes) {
                if (Input.GetKeyDown(key)) pressedKeys.Add(key);
            }
        }

        private void collectNavigationKeys() {
            
        }

        private void handlePressedKeys() {
            bool handled = false;
            foreach (var key in pressedKeys) {
                if (windowManager.accept(key)) handled = true;
            }
            if (handled) return;
            foreach (var key in pressedKeys) {
                if (widgetManager.accept(key)) handled = true;
            }
            if (handled) return;
            // TODO call camera handler
        }
    }
}