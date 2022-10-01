using System.Collections.Generic;
using game.model.component.building;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.view.ui {

    // keeps only one opened window on the screen (active)
    // opens and closes windows by window name
    // passes input to active window
    public class WindowManager : Singleton<WindowManager>, IHotKeyAcceptor {
        // private readonly Dictionary<KeyCode, IWindow> hotKeys = new Dictionary<KeyCode, IWindow>();
        public readonly Dictionary<string, IWindow> windows = new Dictionary<string, IWindow>();
        public IWindow activeWindow;

        public bool accept(KeyCode key) {
            // if (activeWindow == null) return showWindowByKey(key);
            return (activeWindow as IHotKeyAcceptor)?.accept(key) ?? false;
        }
        
        public void addWindow(IWindow window, KeyCode key) {
            // hotKeys.Add(key, window);
            windows.Add(window.getName(), window);
        }

        public bool showWindowByName(string name) {
            return windows.ContainsKey(name) && showWindow(windows[name]);
        }

        public bool toggleWindowByName(string name) {
            return windows.ContainsKey(name) && toggleWindow(windows[name]);
        }
        
        public void closeAll() {
            foreach (var window in windows.Values) {
                closeWindow(window);
            }
        }

        public void closeWindow(IWindow window) {
            activeWindow = null;
            window.close();
            GameView.get().cameraAndMouseHandler.enabled = true;
        }

        public void showWindowForBuilding(EcsEntity entity) {
            if(entity.Has<WorkbenchComponent>()) {
                showWindow(windows["workbench"]);
                // ()activeWindow
            }
        }

        private bool toggleWindow(IWindow window) {
            if (activeWindow == window) {
                closeWindow(window);
            } else {
                showWindow(window);
            }
            return true;
        }
        
        private bool showWindow(IWindow window) {
            closeAll();
            activeWindow = window;
            activeWindow.open();
            GameView.get().cameraAndMouseHandler.enabled = false;
            return true;
        }
    }
}
