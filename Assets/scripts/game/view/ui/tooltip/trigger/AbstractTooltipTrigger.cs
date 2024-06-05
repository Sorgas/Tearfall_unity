using System;
using game.view.ui.tooltip.handler;
using UnityEngine;

namespace game.view.ui.tooltip.trigger {
// Base class for tooltip triggers. Stores data to initialize tooltip.
// Defines that only root tooltip should be updated from Unity engine.
public abstract class AbstractTooltipTrigger : MonoBehaviour {
    protected RectTransform self;
    protected InfoTooltipData data; // data to be used for tooltip

    protected AbstractTooltipHandler tooltip;
    public bool isRoot; // if trigger is root, it will use Unity updates
    public bool fixedPosition; // 
    public Action openCallback; // will be invoked, when tooltip opens
    public Action closeCallback; // will be invoked, when tooltip closes

    public virtual void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }

    public void Update() {
        if (isRoot) updateInternal();
    }

    // custom update, called from parent element in tooltip chain
    // should return true, if trigger has tooltip after update
    public abstract bool updateInternal();

    // should be used by update method
    protected void open(Vector3 position) {
        createTooltip(position);
        openCallback?.Invoke();
    }

    // should open tooltip
    protected abstract void createTooltip(Vector3 position);

    // updates tooltip and calls closing callback if it was closed
    protected bool updateWithCallbacks(bool mouseInTrigger) {
        bool closed = !tooltip.update(mouseInTrigger);
        if(closed) closeCallback?.Invoke();
        return !closed;
    }

    // should check if tooltip is open
    public abstract bool isTooltipOpen();
    
    public void setToolTipData(InfoTooltipData data) => this.data = data;
}
}