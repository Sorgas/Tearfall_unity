using UnityEngine;

namespace game.view.ui.tooltip {
// triggers appearance of infoTooltip when hovered
public class InfoTooltipTrigger : AbstractTooltipTrigger {
    
    // if tooltip is opened, passes update to it. Otherwise checks if mouse is over trigger and opens tooltip
    public override void update() {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        bool mouseInTrigger = self.rect.Contains(localPosition);
        if (tooltip != null) {
            tooltip.update(mouseInTrigger); // updates tooltip chain and can close tooltip
        } else if (tooltip == null && mouseInTrigger) { // mouse in trigger, open tooltip
            tooltip = InfoTooltipGenerator.get().generate(data);
            tooltip.transform.SetParent(self, false);
            tooltip.gameObject.transform.localPosition = localPosition;
        }
    }
}
}