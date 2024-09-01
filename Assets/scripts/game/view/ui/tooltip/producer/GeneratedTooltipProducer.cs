using game.view.ui.tooltip.handler;
using UnityEngine;

namespace game.view.ui.tooltip.producer {
public class GeneratedTooltipProducer : AbstractTooltipProducer {
    public InfoTooltipData data;
    private AbstractTooltipHandler tooltip;
    
    public override AbstractTooltipHandler openTooltip(Vector3 position) {
        tooltip = InfoTooltipGenerator.get().generate(data);
        tooltip.transform.SetParent(tooltipCanvas.transform, false);
        tooltip.gameObject.transform.localPosition = position;
        return tooltip;
    }

    public override void closeTooltip() {
        Destroy(tooltip.gameObject);
    }
}
}