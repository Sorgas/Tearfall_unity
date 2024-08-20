using System;
using game.view.ui.tooltip.handler;
using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// Base class for tooltip triggers. Stores data to initialize tooltip.
// Defines that only root tooltip should be updated from Unity engine.
public abstract class AbstractTooltipTrigger : MonoBehaviour {
    protected RectTransform self;
    protected InfoTooltipData data; // data to be used for tooltip
    protected AbstractTooltipHandler tooltip; // should be null if tooltip is closed
    public bool isRoot; // if trigger is root, it will use Unity updates
    public bool fixedPosition; // place tooltip at mouse position or trigger corner
    public bool keepAfterClose; // destroy tooltip go after close or not. 
    public Action openCallback; // will be invoked, when tooltip opens
    public Action closeCallback; // will be invoked, when tooltip closes
    protected Canvas tooltipCanvas;
    
    public virtual void Awake() {
        self = gameObject.GetComponent<RectTransform>();
        // TODO get canvas from scene
    }

    public void Update() {
        if (isRoot) updateInternal();
    }

    // Custom update, called from parent element in tooltip chain
    // Should return true, if trigger has tooltip after update
    public virtual bool updateInternal() {
        bool condition = openCondition();
        if (isTooltipOpen()) {
            tooltip.update(condition); // can close tooltip
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

    // should check if tooltip should be opened
    protected abstract bool openCondition();

    protected virtual void openTooltip() {
        Vector3 position = getPositionForTooltip();
        if (keepAfterClose && tooltip != null) {
            tooltip.gameObject.SetActive(true);
        } else {
            if (tooltip != null) {
                Destroy(tooltip.gameObject);
            }
            tooltip = InfoTooltipGenerator.get().generate(data);
            tooltip.transform.SetParent(self, false);
            tooltip.gameObject.transform.localPosition = position;
        }
    }

    // called by tooltips
    public virtual void closeTooltip() {
        if (keepAfterClose) {
            tooltip.gameObject.SetActive(false);
        } else {
            Destroy(tooltip.gameObject);
        }
    }
    
    // should check if tooltip is open
    public bool isTooltipOpen() => tooltip != null && tooltip.gameObject.activeSelf;

    protected bool isCanvasFree() => tooltipCanvas.transform.childCount == 0;

    public void setToolTipData(InfoTooltipData data) => this.data = data;

    private Vector3 getPositionForTooltip() {
        if (fixedPosition) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            Vector2 local = new Vector2(rect.xMin, rect.yMax);
            return rectTransform.TransformPoint(local);
        } else {
            return self.InverseTransformPoint(Input.mousePosition);
        }
    }

    public void setTooltip(AbstractTooltipHandler tooltip) {
        this.tooltip = tooltip;
    }
}
}