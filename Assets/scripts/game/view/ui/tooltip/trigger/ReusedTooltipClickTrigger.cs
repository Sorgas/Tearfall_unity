using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// triggers tooltip when trigger object is clicked, instead of mouse hovered.
// expects tooltip to hide GO instead of destroying.
public class ReusedTooltipClickTrigger : ClickingTooltipTrigger {

    protected override void openTooltip(Vector3 position) {
        Vector3 worldPosition = fixedPosition
            ? getTopLeftCorner()
            : Input.mousePosition;
        Vector3 localPosition = tooltip.transform.parent.InverseTransformPoint(worldPosition);
        tooltip.gameObject.transform.localPosition = localPosition;
        tooltip.gameObject.SetActive(true);
        tooltip.parent = this; // link tooltip to this trigger
    }

    protected override bool isTooltipOpen() {
        return tooltip.parent == this && tooltip.gameObject.activeSelf;
    }

    private Vector3 getTopLeftCorner() {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        Vector2 local = new Vector2(rect.xMin, rect.yMax);
        return rectTransform.TransformPoint(local);
    }
}
}