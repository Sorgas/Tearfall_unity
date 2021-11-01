using System.Collections.Generic;
using UnityEngine;

namespace game.view.ui.toolbar {
    
    // holds and manages sub-panels for toolbar panel
    // only one sub-panel can be enabled at once
    // passes input to sub-panels
    public class ToolbarPanelHandler : MonoBehaviour, IHotKeyAcceptor, IWindow{
        protected Dictionary<string, ToolbarPanelHandler> subPanels = new Dictionary<string, ToolbarPanelHandler>();
        
        public bool accept(KeyCode key) {
            foreach (var toolbarPanelHandler in subPanels.Values) {
                if (toolbarPanelHandler.gameObject.activeSelf) {
                    return toolbarPanelHandler.accept(key);
                }
            }
            // buttons handled in realisation;
            return false;
        }

        public void close() {
            closeAllPanels();
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public void openPanel(string name) {
            closeAllPanels();
            subPanels[name]?.open();
        }

        public void closeAllPanels() {
            foreach (var toolbarPanelHandler in subPanels.Values) {
                toolbarPanelHandler.close();
            }
        }
    }
}