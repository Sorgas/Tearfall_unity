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
    public Button doorButton;
    public TMP_Dropdown blockTypeDropdown;
    public TMP_Dropdown materialDropdown;

    private string selectedBlockType = "wall";
    private string selectedMaterial;
    
    private void Start() {
        blockTypeDropdown.AddOptions(new List<string> { "wall", "ramp", "floor", "stairs", "downstairs" });
        blockTypeDropdown.onValueChanged.AddListener(number => {
            selectedBlockType = blockTypeDropdown.options[number].text;
            setDebugTool();
        });
        List<string> materials = MaterialMap.get().all
            .Where(material => material.tags.Contains("stone"))
            .Select(material => material.name)
            .ToList();
        materialDropdown.AddOptions(materials);
        materialDropdown.onValueChanged.AddListener(number => {
            selectedMaterial = materialDropdown.options[number].text;
            setDebugTool();
        });
        // blockTypeDropdown.onValueChanged.Invoke(0);
        materialDropdown.onValueChanged.Invoke(0);
        wallButton.onClick.AddListener(() => MouseToolManager.get().setDebug(selectedBlockType, selectedMaterial));
        doorButton.onClick.AddListener(() => MouseToolManager.get().setDebugDoor());
    }

    private void setDebugTool() {
        MouseToolManager.get().debugTileTool.set(selectedBlockType, selectedMaterial);
    }
}
}