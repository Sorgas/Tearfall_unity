using game.view.ui.tooltip;
using game.view.ui.tooltip.trigger;
using TMPro;
using types.unit.disease;
using UnityEngine;
using UnityEngine.UIElements;

namespace game.view.ui.unit_menu {
public class DiseaseRowHandler : MonoBehaviour {
    public TextMeshProUGUI diseaseName;
    public TextMeshProUGUI diseaseProgress;
    public Image diseaseIcon;
    private InfoTooltipTrigger tooltipTrigger;
    private UnitDisease unitDisease;
    
    // public void Start() {
    // }

    public void Update() {
        udpateDiseaseProgress();
    }
    
    public void init(UnitDisease unitDisease) {
        this.unitDisease = unitDisease;
        diseaseName.text = unitDisease.type.name;
        tooltipTrigger = gameObject.GetComponent<InfoTooltipTrigger>();
        tooltipTrigger.isRoot = true;
        tooltipTrigger.setToolTipData(new InfoTooltipData("disease", unitDisease));
    }

    private void udpateDiseaseProgress() {
        if (unitDisease != null) {
            diseaseProgress.text = $"{(unitDisease.progress * 100):##.##}%";
        }
    }
}
}