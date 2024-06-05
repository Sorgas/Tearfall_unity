using game.view.ui.tooltip;
using game.view.ui.tooltip.trigger;
using TMPro;
using types.unit.disease;
using UnityEngine;
using UnityEngine.UIElements;

namespace game.view.ui.unit_menu {
public class DiseaseRowHandler : MonoBehaviour {
    public TextMeshProUGUI diseaseName;
    public Image diseaseIcon;
    private InfoTooltipTrigger tooltipTrigger;

    // public void Start() {
    // }
    
    public void init(UnitDisease unitDisease) {
        diseaseName.text = unitDisease.type.name;
        tooltipTrigger = gameObject.GetComponent<InfoTooltipTrigger>();
        tooltipTrigger.isRoot = true;
        tooltipTrigger.setToolTipData(new InfoTooltipData("disease", unitDisease));
    }
}
}