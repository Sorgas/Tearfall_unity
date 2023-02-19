using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.debug_tools {
    public class DebugToolsHandler : MonoBehaviour{
        public Button toolsButton;
        public RectTransform toolsPanel;

        private void Start() {
            toolsButton.onClick.AddListener(() => {
                Debug.Log("press");
                toolsPanel.gameObject.SetActive(!toolsPanel.gameObject.activeSelf);
            });
        }
    }
}