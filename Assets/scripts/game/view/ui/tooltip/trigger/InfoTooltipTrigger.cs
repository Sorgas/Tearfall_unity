using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// When mouse hovered, generates tooltip by provided TooltipData and shows it.
public class InfoTooltipTrigger : HoveringTooltipTrigger {
    
    // if tooltip is opened, passes update to it. Otherwise checks if mouse is over trigger and opens tooltip
    public override bool update() {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        bool mouseInTrigger = self.rect.Contains(localPosition);
        if (tooltip != null) {
            tooltip.update(mouseInTrigger); // updates tooltip chain and can close tooltip
        } else if (tooltip == null && mouseInTrigger) { // mouse in trigger, open tooltip
            openWithCallbacks(localPosition);
            return true;
        }
        return false;
    }

    protected override void openTooltip(Vector3 position) {
        tooltip = InfoTooltipGenerator.get().generate(data);
        tooltip.transform.SetParent(self, false);
        tooltip.gameObject.transform.localPosition = position;
    }

    protected override bool isTooltipOpen() {
        return tooltip != null;
    }
}
}