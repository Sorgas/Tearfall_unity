using game.view.ui.tooltip.handler;
using UnityEngine;

namespace game.view.ui.tooltip.producer {
public class ReusedTooltipProducer : AbstractTooltipProducer {
    public AbstractTooltipHandler tooltip; // should be set in editor
    
    public override AbstractTooltipHandler openTooltip(Vector3 position) {
        tooltip.gameObject.SetActive(true);
        tooltip.transform.SetParent(tooltipCanvas.transform, true);
        return tooltip;
    }

    public override void closeTooltip() {
        tooltip.gameObject.SetActive(false);
        tooltip.transform.SetParent(transform, true);
    }
}
}