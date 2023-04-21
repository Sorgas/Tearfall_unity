using game.input;
using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.ui.util {
    // menu to be
    public abstract class WindowManagerMenu : MbWindow, INamed, IHotKeyAcceptor {
        
        public abstract string getName();

        public bool accept(KeyCode key) {
            if (key == KeyCode.Q) {
                WindowManager.get().closeWindow(getName());
                MouseToolManager.get().reset();
                return true;
            }
            return false;
        }
    }
}