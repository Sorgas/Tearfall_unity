using UnityEngine;

namespace game.view.ui.tooltip {
// triggers appearance of infoTooltip when hovered
public class InfoTooltipTrigger : MonoBehaviour
        // , IPointerEnterHandler, IPointerExitHandler
{
    public InfoTooltipHandler tooltip;
    public bool isRoot;
    public bool fixedPosition; // TODO create tooltip at current mouse position or at the corner of trigger
    private RectTransform self;
    private InfoTooltipData data; // data to be used for tooltip

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }
    
    public void Update() {
        if (isRoot) update();
    }

    // custom update, called from root tooltip trigger
    public void update() {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        bool mouseInTrigger = self.rect.Contains(localPosition);
        if (tooltip != null) {
            tooltip.update(mouseInTrigger); // updates tooltip chain and can close tooltip
        }
        if (tooltip == null && mouseInTrigger) { // mouse in trigger, open tooltip
            tooltip = InfoTooltipGenerator.get().generate(data);
            tooltip.transform.SetParent(self, false);
            tooltip.gameObject.transform.localPosition = localPosition;
        }
    }

    public void setToolTipData(InfoTooltipData data) => this.data = data;
    
    // public void OnPointerEnter(PointerEventData eventData) {
    //     Debug.Log("mouse enters trigger");
    //     Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
    //     bool mouseInTrigger = self.rect.Contains(localPosition);
    //     if (tooltip == null) {
    //         tooltip = InfoTooltipGenerator.get().generate(data);
    //         tooltip.transform.SetParent(parent, false);
    //         tooltip.gameObject.transform.localPosition = localPosition;
    //     }
    // }
    //
    // public void OnPointerExit(PointerEventData eventData) {
    //     Debug.Log("mouse exits trigger");
    //     Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
    //     bool mouseInTrigger = self.rect.Contains(localPosition);
    //
    //     Destroy(tooltip.gameObject);
    //
    //     // if (tooltip != null) {
    //     //     tooltip.update(mouseInTrigger); // updates tooltip chain and can close tooltip
    //     // }
    // }
}
}