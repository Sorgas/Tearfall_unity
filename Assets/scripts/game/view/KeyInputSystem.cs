﻿using System.Collections.Generic;
using game.view.ui;
using UnityEngine;
using util.lang;

namespace game.view {

    // dispatches all key presses in the game
    public class KeyInputSystem : Singleton<KeyInputSystem> {
        private List<KeyCode> keyCodes;
        public WindowManager windowManager = WindowManager.get();
        public WidgetManager widgetManager = WidgetManager.get();
        private List<KeyCode> pressedKeys = new List<KeyCode>();

        public KeyInputSystem() {
            KeyCode[] keys = {
                KeyCode.Q, KeyCode.E, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
                KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
                KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M
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

        private void handlePressedKeys() {
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