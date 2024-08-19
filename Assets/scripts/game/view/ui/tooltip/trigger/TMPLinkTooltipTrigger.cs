using TMPro;
using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// Handles TMP text with multiple links. Opens tooltip when link in text is hovered.
// This class is 'stateful': linkIndex is set in openCondition() and could then be used in openTooltip()
public class TMPLinkTooltipTrigger : AbstractTooltipTrigger {
    private TextMeshProUGUI text;
    private int hoveredLinkIndex = -1; // currently hovered link
    private int openTooltipIndex = -1; // currently open tooltip
    
    public new void Awake() {
        base.Awake();
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    
    // if tooltip is open, passes update to it. Otherwise, checks if mouse is over any link in text and opens corresponding tooltip
    public override bool updateInternal() {
        bool condition = openCondition(); // sets hoveredLinkIndex
        if (isTooltipOpen()) {
            tooltip.update(openTooltipIndex == hoveredLinkIndex); // can close tooltip
            if (!isTooltipOpen()) {
                closeCallback?.Invoke();
            }
        } else {
            if(condition && isCanvasFree()) {
                openTooltip();
                openCallback?.Invoke();
            }
        }
        return tooltip != null;
    }

    // create tooltip when any link hovered
    protected override bool openCondition() {
        hoveredLinkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
        return hoveredLinkIndex >= 0;
    }

    protected override void openTooltip() {
        Debug.Log("mouse in link " + hoveredLinkIndex);
        TMP_LinkInfo linkInfo = text.textInfo.linkInfo[hoveredLinkIndex];
        string linkId = linkInfo.GetLinkID();
        tooltip = InfoTooltipGenerator.get().generateFromLink(linkId);
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        tooltip.transform.SetParent(self, false);
        tooltip.gameObject.transform.localPosition = localPosition;
        openTooltipIndex = hoveredLinkIndex;
    }

    public override void closeTooltip() {
        base.closeTooltip();
        openTooltipIndex = -1;
    }
}
}