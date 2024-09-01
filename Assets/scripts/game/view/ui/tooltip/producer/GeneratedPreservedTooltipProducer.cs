using game.view.ui.tooltip.handler;
using UnityEngine;

namespace game.view.ui.tooltip.producer {
// generates tooltip by InfoTooltipData. When closed, deactivates tooltip go.
public class GeneratedPreservedTooltipProducer : AbstractTooltipProducer {
    private AbstractTooltipHandler tooltip;
    
    public override AbstractTooltipHandler openTooltip(Vector3 position) {
        if (tooltip == null) {
            tooltip = InfoTooltipGenerator.get().generate(data);
        }
        tooltip.gameObject.SetActive(true);
        tooltip.transform.SetParent(tooltipCanvas.transform, false);
        tooltip.gameObject.transform.localPosition = position;
        return tooltip;
    }

    public override void closeTooltip() {
        tooltip.gameObject.SetActive(false);
        tooltip.transform.SetParent(transform, true);
    }
}
}