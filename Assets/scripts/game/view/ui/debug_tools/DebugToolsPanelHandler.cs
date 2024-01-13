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
    public Button bedButton;
    public TMP_Dropdown blockTypeDropdown;
    public TMP_Dropdown materialDropdown;

    public void init() {
        Debug.Log("debug panel inited");
        initMaterialDropdown();
        initBlockTypeDropdown();
        wallButton.onClick.AddListener(() => MouseToolManager.get().setDebugTileTool(
            blockTypeDropdown.options[blockTypeDropdown.value].text,
            materialDropdown.options[materialDropdown.value].text));
        doorButton.onClick.AddListener(() => MouseToolManager.get().setDebugBuildingTool("door"));
        bedButton.onClick.AddListener(() => MouseToolManager.get().setDebugBuildingTool("bed"));
    }

    private void initBlockTypeDropdown() {
        blockTypeDropdown.AddOptions(new List<string> { "wall", "ramp", "floor", "stairs", "downstairs" });
        blockTypeDropdown.onValueChanged.Invoke(0);
    }

    private void initMaterialDropdown() {
        List<string> materials = MaterialMap.get().all
            .Where(material => material.tags.Contains("stone"))
            .Select(material => material.name)
            .ToList();
        materialDropdown.AddOptions(materials);
        materialDropdown.onValueChanged.Invoke(0);
    }
}
}