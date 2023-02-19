using System.Collections.Generic;
using System.Linq;
using game.view.system.mouse_tool;
using TMPro;
using types.material;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.debug_tools {
    public class DebugToolsPanelHandler : MonoBehaviour {
        public Button wallButton;
        public TMP_Dropdown blockTypeDropdown;
        public TMP_Dropdown materialDropdown;

        private void Start() {
            blockTypeDropdown.AddOptions(new List<string> { "wall", "ramp", "floor", "stairs", "downstairs" });
            List<string> materials = MaterialMap.get().all
                .Where(material => material.tags.Contains("stone"))
                .Select(material => material.name)
                .ToList();
            materialDropdown.AddOptions(materials);
            wallButton.onClick.AddListener(() => MouseToolManager.get().setDebug("wall", "marble"));
        }
        
        
    }
}