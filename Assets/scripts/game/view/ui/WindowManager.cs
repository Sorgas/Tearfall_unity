using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.ui {

    // keeps only one opened widget on the screen
    // passes input to active widget
    public class WindowManager : Singleton<WindowManager>, IHotKeyAcceptor {
        private readonly Dictionary<KeyCode, IWindow> hotKeys = new Dictionary<KeyCode, IWindow>();
        public readonly Dictionary<string, IWindow> windows = new Dictionary<string, IWindow>();
        public IWindow activeWindow;

        public bool accept(KeyCode key) {
            if (activeWindow == null) return showWindowByKey(key);
            return (activeWindow as IHotKeyAcceptor)?.accept(key) ?? false;
        }
        
        public void addWindow(IWindow window, KeyCode key) {
            hotKeys.Add(key, window);
            windows.Add(window.getName(), window);
        }

        public bool showWindowByKey(KeyCode key) {
            return hotKeys.ContainsKey(key) && showWindow(hotKeys[key]);
        }

        public bool showWindowByName(string name) {
            return windows.ContainsKey(name) && showWindow(windows[name]);
        }
        
        public void closeAll() {
            foreach (var window in windows.Values) {
                closeWindow(window);
            }
        }

        public void closeWindow(IWindow window) {
            activeWindow = null;
            window.close();
            GameView.get().cameraHandler.enabled = true;
        }

        private bool showWindow(IWindow window) {
            closeAll();
            activeWindow = window;
            activeWindow.open();
            GameView.get().cameraHandler.enabled = false;
            return true;
        }
    }
}