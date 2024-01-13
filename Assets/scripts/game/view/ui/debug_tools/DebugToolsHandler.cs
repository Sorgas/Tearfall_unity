using UnityEngine;
using UnityEngine.UI;
using util.lang;

namespace game.view.ui.debug_tools {
    public class DebugToolsHandler : MonoBehaviour, Initable {
        public Button toolsButton;
        public DebugToolsPanelHandler toolsPanel;

        private void Start() {
            toolsButton.onClick.AddListener(() => {
                Debug.Log("press");
                toolsPanel.gameObject.SetActive(!toolsPanel.gameObject.activeSelf);
            });
        }

        public void init() {
            toolsPanel.init();
        }
    }
}