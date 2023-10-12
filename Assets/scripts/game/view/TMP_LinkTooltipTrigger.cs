using game.view.ui.tooltip;
using TMPro;
using UnityEngine;

namespace game.view {
// tries to detect when mouse is hovering over links in text, and display corresponding tooltip. 
// tooltips to show are stored in InfoTooltipRegistry
// 
public class TMP_LinkTooltipTrigger : MonoBehaviour {
    private TextMeshProUGUI text;
    private InfoTooltipHandler tooltip;
    private int currentLink;
    private RectTransform self;
    public RectTransform parent; // tooltip will be child of this transform

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
        text = gameObject.GetComponent<TextMeshProUGUI>();
        parent = gameObject.GetComponent<RectTransform>();
    }

    public void Update() {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
        if (tooltip == null) { // try to open tooltip
            if (linkIndex >= 0) {
                Debug.Log("mouse in link " + linkIndex);
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                string linkId = linkInfo.GetLinkID();
                Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
                tooltip = InfoTooltipGenerator.get().generate(new InfoTooltipData("dummy"));
                tooltip.transform.SetParent(parent, false);
                tooltip.gameObject.transform.localPosition = localPosition;
                currentLink = linkIndex;
            }
        } else {
            tooltip.update(linkIndex == currentLink); // can close tooltip
            // if (tooltip.gameObject == null) { // tooltip destroyed
            //     
            // }
            // if (linkIndex == -1) {
            //     Debug.Log("mouse outside link");
            //     Destroy(tooltip.gameObject);
            // }    
        }
    }
}
}