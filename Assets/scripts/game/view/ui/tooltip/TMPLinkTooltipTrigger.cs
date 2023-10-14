using TMPro;
using UnityEngine;

namespace game.view.ui.tooltip {

// Handles TMP text. Opens tooltip when link in text is hovered.
public class TMPLinkTooltipTrigger : AbstractTooltipTrigger {
    private TextMeshProUGUI text;
    private int currentLink = -1; 

    public new void Awake() {
        base.Awake();
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // if tooltip is open, passes update to it. Otherwise, checks if mouse is over any link in text and opens corresponding tooltip
    public override void update() {
        Debug.Log("updating text trigger");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
        if (tooltip == null) { // try to open tooltip
            if (linkIndex >= 0) {
                Debug.Log("mouse in link " + linkIndex);
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                string linkId = linkInfo.GetLinkID();
                Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
                tooltip = InfoTooltipGenerator.get().generateFromLink(linkId);
                tooltip.transform.SetParent(self, false);
                tooltip.gameObject.transform.localPosition = localPosition;
                currentLink = linkIndex;
            }
        } else {
            tooltip.update(linkIndex == currentLink); // can close tooltip
            if (tooltip == null) currentLink = -1;
        }
    }
}
}