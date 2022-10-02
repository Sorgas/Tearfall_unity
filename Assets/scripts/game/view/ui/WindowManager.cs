using System.Collections.Generic;
using game.model.component.building;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.view.ui {

    // keeps only one opened window on the screen (active)
    // opens and closes windows by window name
    // passes input to active window
    // when window is shown 
    public class WindowManager : Singleton<WindowManager>, IHotKeyAcceptor {
        public readonly Dictionary<string, IWindow> windows = new Dictionary<string, IWindow>(); // windows by name
        public IWindow activeWindow;

        public bool accept(KeyCode key) {
            return (activeWindow as IHotKeyAcceptor)?.accept(key) ?? false;
        }

        public void addWindow(IWindow window) {
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
                IWindow window = windows["workbench"];
                ((WorkbenchWindowHandler) window).init(entity);
                showWindow(window, false);
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
        
        private bool showWindow(IWindow window) => showWindow(window, true);

        private bool showWindow(IWindow window, bool disableCamera) {
            closeAll();
            Debug.Log("window " + window.getName() + " shown.");
            activeWindow = window;
            activeWindow.open();
            if(disableCamera) GameView.get().cameraAndMouseHandler.enabled = false;
            return true;
        }
    }
}
