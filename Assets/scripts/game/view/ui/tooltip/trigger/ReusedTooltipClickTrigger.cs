using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// triggers tooltip when trigger object is clicked, instead of mouse hovered.
// expects tooltip to hide GO instead of destroying.
public class ReusedTooltipClickTrigger : ClickingTooltipTrigger {

    public override void openTooltip(Vector3 position) {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        tooltip.transform.SetParent(self, false);
        if(!fixedPosition) tooltip.gameObject.transform.localPosition = localPosition;
        Debug.Log("tooltip opened");
    }

    protected override bool isTooltipOpen() {
        return tooltip.gameObject.activeSelf;
    }
}
}