using UnityEngine;

namespace game.view.ui.tooltip.trigger {
public class HoveringTooltipTrigger : AbstractTooltipTrigger {
    private float delay;
    
    // if tooltip is hovered for more than tooltipShowDelay
    protected override bool openCondition() {
        if (self.rect.Contains(self.InverseTransformPoint(Input.mousePosition)) && isCanvasFree()) {
            delay += Time.unscaledDeltaTime;
            if (delay > GlobalSettings.tooltipShowDelay) {
                return true;
            }
        } else {
            delay = 0;
        }
        return false;
    }

    protected override bool closeCondition() {
        if (!self.rect.Contains(self.InverseTransformPoint(Input.mousePosition))) {
            delay = 0;
            return true;
        }
        return false;
    }
}
}