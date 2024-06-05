using UnityEngine;

namespace game.view.ui.tooltip.trigger {
public abstract class HoveringTooltipTrigger : AbstractTooltipTrigger {
    private float delay;
    
    // if tooltip is opened, passes update to it. Otherwise checks if mouse is over trigger and opens tooltip
    public override bool updateInternal() {
        bool mouseInTrigger = self.rect.Contains(self.InverseTransformPoint(Input.mousePosition));
        // updates tooltip chain and can close tooltip
        if (isTooltipOpen()) return updateWithCallbacks(mouseInTrigger);
        if (mouseInTrigger && GameView.get().sceneElements.tooltipCanvas.transform.childCount == 0) {
            delay += Time.unscaledDeltaTime;
            if (delay > GlobalSettings.tooltipShowDelay) {
                open(Input.mousePosition);
                delay = 0;
                return true;
            }
        }
        return false;
    }
}
}