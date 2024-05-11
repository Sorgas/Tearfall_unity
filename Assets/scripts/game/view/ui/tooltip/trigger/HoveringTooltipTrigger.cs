using UnityEngine;

namespace game.view.ui.tooltip.trigger {
public abstract class HoveringTooltipTrigger : AbstractTooltipTrigger {
    // if tooltip is opened, passes update to it. Otherwise checks if mouse is over trigger and opens tooltip
    public override bool update() {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        bool mouseInTrigger = self.rect.Contains(localPosition);
        if (isTooltipOpen()) {
            // updates tooltip chain and can close tooltip
            updateWithCallbacks(mouseInTrigger);
        } else if (tooltip == null && mouseInTrigger) { // mouse in trigger, open tooltip
            openWithCallbacks(localPosition);
            return true;
        }
        return false;
    }
}
}