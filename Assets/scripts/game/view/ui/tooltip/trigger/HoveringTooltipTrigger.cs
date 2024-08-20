using UnityEngine;

namespace game.view.ui.tooltip.trigger {
public class HoveringTooltipTrigger : AbstractTooltipTrigger {
    private float delay;
    
    protected override bool openCondition() {
        bool mouseInTrigger = self.rect.Contains(self.InverseTransformPoint(Input.mousePosition));
        if (mouseInTrigger && isCanvasFree()) { // can open tooltip
            delay += Time.unscaledDeltaTime;
            if (delay > GlobalSettings.tooltipShowDelay) {
                delay = 0;
                return true;
            }
        } else {
            delay = 0;
        }
        return false;
    }
}
}