using System.Collections.Generic;
using game.model.component.building;
using game.model.component.unit;
using game.view;
using game.view.ui.unit_menu;
using game.view.ui.util;
using game.view.ui.workbench;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.input {
// keeps only one opened window on the screen (active)
    // opens and closes windows by window name
    // passes input to active window
    public class WindowManager : Singleton<WindowManager>, IHotKeyAcceptor {
        public readonly Dictionary<string, GameWindow> windows = new(); // windows by name
        public string activeWindowName = "";
        public GameWindow activeWindow;
        private bool debug = false;
        
        public bool accept(KeyCode key) {
            return activeWindow?.accept(key) ?? false;
        }

        public void addWindow(GameWindow window) {
            windows.Add(window.getName(), window);
        }

        public void showWindowByName(string name, bool disableCamera) {
            if (windows.ContainsKey(name)) {
                showWindow(name, disableCamera);
                WidgetManager.get().resetAllWidgets();
            }
        }

        public void toggleWindowByName(string name) {
            if(windows.ContainsKey(name)) toggleWindow(name);
        }

        public void closeWindow(string name) {
            log("closing window " + name);
            activeWindow = null;
            activeWindowName = "";
            if (!windows.ContainsKey(name)) return;
            windows[name].close();
            GameView.get().cameraAndMouseHandler.enabled = true;
        }

        public void closeAll() {
            foreach (string name in windows.Keys) {
                closeWindow(name);
            }
        }

        public void showWindowForBuilding(EcsEntity entity) {
            if(entity.Has<WorkbenchComponent>()) {
                INamed window = windows[WorkbenchWindowHandler.name];
                ((WorkbenchWindowHandler) window).init(entity);
                showWindow(WorkbenchWindowHandler.name, false);
            }
        }

        public void showWindowForUnit(EcsEntity entity) {
            if(entity.Has<UnitComponent>()) {
                INamed window = windows[UnitMenuHandler.NAME];
                ((UnitMenuHandler) window).showUnit(entity);
                showWindow(UnitMenuHandler.NAME, false);
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
            closeWindow(activeWindowName);
            GameWindow window = windows[name];
            log("window " + window.getName() + " shown.");
            activeWindow = window;
            activeWindowName = name;
            activeWindow.open();
            if(disableCamera) GameView.get().cameraAndMouseHandler.enabled = false;
            // TODO cancel selection
            return true;
        }

        private void log(string message) {
            if(debug) Debug.Log("[WindowManager] " + message);
        }
    }
}
