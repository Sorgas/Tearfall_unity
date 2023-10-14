using UnityEngine;

namespace game.view.ui.tooltip {
// base class for tooltip triggers
public abstract class AbstractTooltipTrigger : MonoBehaviour {
    public InfoTooltipHandler tooltip; // currently opened tooltip
    public bool isRoot; // if trigger is root, it will use Unity updates
    // TODO create tooltip at current mouse position or at the corner of trigger
    public bool fixedPosition;
    protected InfoTooltipData data; // data to be used for tooltip
    protected RectTransform self;

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }

    public void Update() {
        if (isRoot) update();
    }

    // custom update, called from parent element in tooltip chain
    public abstract void update();

    public void setToolTipData(InfoTooltipData data) => this.data = data;
}
}