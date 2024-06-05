using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// When mouse hovered, generates tooltip by provided TooltipData and shows it.
public class InfoTooltipTrigger : HoveringTooltipTrigger {

    protected override void createTooltip(Vector3 position) {
        tooltip = InfoTooltipGenerator.get().generate(data);
        Transform canvas = GameView.get().sceneElements.tooltipCanvas.transform;
        tooltip.transform.SetParent(canvas, false);
        tooltip.gameObject.transform.localPosition = canvas.InverseTransformPoint(position);
    }

    public override bool isTooltipOpen() {
        return tooltip != null;
    }
}
}