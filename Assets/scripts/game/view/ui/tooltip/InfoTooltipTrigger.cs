using UnityEngine;

namespace game.view.ui.tooltip {
// triggers appearance of infoTooltip when hovered
public class InfoTooltipTrigger : MonoBehaviour {
    public RectTransform parent; // tooltip will be child of this transform
    public InfoTooltipHandler tooltip;
    public bool isRoot;
    private RectTransform self;
    private InfoTooltipData data;

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }
    
    public void Update() {
        Debug.Log("trigger update");
        // if (isRoot) 
            update();
    }

    // custom update, called from root tooltip trigger
    public void update() {
        if (tooltip != null) {
            Debug.Log("updating tooltip");
            tooltip.update(); // can close tooltip
        }
        if (tooltip == null) {
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            Debug.Log("localPosition " + localPosition);
            if (self.rect.Contains(localPosition)) {
                Debug.Log("mouse in trigger");
                tooltip = InfoTooltipGenerator.get().generate(data);
                tooltip.transform.parent = parent; 
                tooltip.gameObject.transform.localPosition = localPosition;
            }
        }
    }

    public void setToolTipData(InfoTooltipData data) => this.data = data;
}
}