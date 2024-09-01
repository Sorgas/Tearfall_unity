using System;
using System.Linq;
using game.view.ui.tooltip.handler;
using game.view.ui.tooltip.producer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.view.ui.tooltip.trigger {
// Tooltip triggers are elements which initiate appearance of tooltip when some event occurs.
// Stores data to initialize tooltip. Defines that only root tooltip should be updated from Unity engine.
public abstract class AbstractTooltipTrigger : MonoBehaviour {
    public bool isRoot; // if trigger is root, it will use Unity updates
    public bool fixedPosition; // place tooltip at mouse position or trigger corner

    protected RectTransform self;
    protected AbstractTooltipHandler tooltip; // should be null if tooltip is closed

    public Action openCallback; // will be invoked, when tooltip opens
    public Action closeCallback; // will be invoked, when tooltip closes
    private Canvas tooltipCanvas;
    private AbstractTooltipProducer producer;
    
    public virtual void Awake() {
        self = gameObject.GetComponent<RectTransform>();
        producer = gameObject.GetComponent<AbstractTooltipProducer>();
        tooltipCanvas = SceneManager.GetActiveScene()
            .GetRootGameObjects().First(go => go.name.Equals("TooltipCanvas")).GetComponent<Canvas>();
    }

    public void Update() {
        if (isRoot) updateInternal();
    }

    // Custom update, called from parent element in tooltip chain
    // Should return true, if trigger has tooltip after update
    public virtual bool updateInternal() {
        if (isTooltipOpen()) {
            bool preserve = !closeCondition(); // if tooltip cannot be closed, it should be preserved
            tooltip.update(preserve); // can close tooltip
        } else {
            if (openCondition()) {
                openTooltip();
            }
        }
        return tooltip != null;
    }

    // should return true, when tooltip should be opened (e.g. trigger hovered)
    protected abstract bool openCondition();

    // should return true, when tooltip can be closed (e.g. trigger not hovered)
    protected abstract bool closeCondition();
    
    // sets tooltip from tooltipInstance or creates from generator
    protected virtual void openTooltip() {
        tooltip = producer.openTooltip(getPositionForTooltip());
        openCallback?.Invoke();
        tooltip.parent = this;
    }

    // called by tooltips
    public virtual void closeTooltip() {
        producer.closeTooltip();
        closeCallback?.Invoke();
        tooltip = null;
    }

    // should check if tooltip is open
    public bool isTooltipOpen() => tooltip != null && tooltip.gameObject.activeSelf;

    protected bool isCanvasFree() => tooltipCanvas.transform.childCount == 0;

    private Vector3 getPositionForTooltip() {
        if (fixedPosition) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            Vector2 local = new Vector2(rect.xMin, rect.yMax);
            return rectTransform.TransformPoint(local);
        }
        return tooltipCanvas.transform.InverseTransformPoint(Input.mousePosition);
    }

    public void setTooltip(AbstractTooltipHandler tooltip) {
        this.tooltip = tooltip;
    }
}
}