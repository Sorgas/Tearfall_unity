using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.ui {

    // keeps only one opened widget on the screen
    // passes input to active widget
    public class WindowManager : Singleton<WindowManager>, IHotKeyAcceptor {
        public readonly Dictionary<string, IWindow> windows = new Dictionary<string, IWindow>();
        public IWindow activeWindow;

        public bool accept(KeyCode key) {
            return (activeWindow as IHotKeyAcceptor)?.accept(key) ?? false;
        }

        public void showWindow(string name) {
            closeAll();
            if (windows.ContainsKey(name)) {
                activeWindow = windows[name];
                activeWindow.open();
                GameView.get().cameraHandler.enabled = false;
            }
        }

        public void closeAll() {
            foreach (var window in windows.Values) {
                window.close();
            }
            GameView.get().cameraHandler.enabled = true;
        }
    }
}