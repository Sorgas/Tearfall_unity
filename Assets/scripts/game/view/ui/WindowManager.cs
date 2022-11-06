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
        public string activeWindowName;

        public bool accept(KeyCode key) {
            return (activeWindow as IHotKeyAcceptor)?.accept(key) ?? false;
        }

        public void addWindow(IWindow window) {
            windows.Add(window.getName(), window);
        }

        public bool showWindowByName(string name, bool disableCamera) {
            return windows.ContainsKey(name) && showWindow(name, disableCamera);
        }

        public bool toggleWindowByName(string name) {
            return windows.ContainsKey(name) && toggleWindow(name);
        }
        
        public void closeAll() {
            foreach (var name in windows.Keys) {
                closeWindow(name);
            }
        }

        public void closeWindow(string name) {
            activeWindow = null;
            activeWindowName = null;
            windows[name].close();
            GameView.get().cameraAndMouseHandler.enabled = true;
        }

        public void showWindowForBuilding(EcsEntity entity) {
            if(entity.Has<WorkbenchComponent>()) {
                IWindow window = windows[WorkbenchWindowHandler.name];
                ((WorkbenchWindowHandler) window).init(entity);
                showWindow(WorkbenchWindowHandler.name, false);
            }
        }

        private bool toggleWindow(string name) {
            if (activeWindowName == name) {
                closeWindow(name);
            } else {
                showWindow(name);
            }
            return true;
        }
        
        private bool showWindow(string name) => showWindow(name, true);

        public bool showWindow(string name, bool disableCamera) {
            closeAll();
            IWindow window = windows[name];
            Debug.Log("window " + window.getName() + " shown.");
            activeWindow = window;
            activeWindowName = name;
            activeWindow.open();
            if(disableCamera) GameView.get().cameraAndMouseHandler.enabled = false;
            return true;
        }
    }
}
