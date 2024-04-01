using System;
using game.view.ui.tooltip.handler;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.tooltip.trigger {
// Base class for tooltip triggers. Stores data to initialize tooltip.
// Defines that only root tooltip should be updated from Unity engine.
public abstract class AbstractTooltipTrigger : MonoBehaviour {
    public AbstractTooltipHandler tooltip; // currently opened tooltip
    public bool isRoot; // if trigger is root, it will use Unity updates
    // TODO create tooltip at current mouse position or at the corner of trigger
    public bool fixedPosition;
    protected InfoTooltipData data; // data to be used for tooltip
    protected RectTransform self;
    public Action openCallback; // will be invoked, when tooltip opens
    public Action closeCallback; // will be invoked, when tooltip closes

    public virtual void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }

    public void Update() {
        if (isRoot) update();
    }

    // custom update, called from parent element in tooltip chain
    public abstract bool update();

    // should open tooltip
    protected abstract void openTooltip(Vector3 position);

    protected void openWithCallbacks(Vector3 position) {
        openTooltip(position);
        openCallback?.Invoke();
    }

    protected void updateWithCallbacks(bool mouseInTrigger) {
        if (!tooltip.update(mouseInTrigger)) closeCallback?.Invoke();
    }

    // should check if tooltip is 
    protected abstract bool isTooltipOpen();
    
    public void setToolTipData(InfoTooltipData data) => this.data = data;
}

// opens tooltip when trigger is hovered
public abstract class HoveringTooltipTrigger : AbstractTooltipTrigger {
    // if tooltip is opened, passes update to it. Otherwise checks if mouse is over trigger and opens tooltip
    public override bool update() {
        Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
        bool mouseInTrigger = self.rect.Contains(localPosition);
        if (isTooltipOpen()) {
            // updates tooltip chain and can close tooltip
            updateWithCallbacks(mouseInTrigger);
        } else if (tooltip == null && mouseInTrigger) { // mouse in trigger, open tooltip
            openWithCallbacks(localPosition);
            return true;
        }
        return false;
    }
}

[RequireComponent(typeof(Button))]
// opens tooltip when trigger clicked
public abstract class ClickingTooltipTrigger : AbstractTooltipTrigger {
    public override void Awake() {
        base.Awake();
        gameObject.GetComponent<Button>().onClick.AddListener(() => {
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            openWithCallbacks(localPosition);
            openCallback?.Invoke();
        });
    }
    
    public override bool update() {
        if (isTooltipOpen()) {
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            bool mouseInTrigger = self.rect.Contains(localPosition);
            // updates tooltip chain and can close tooltip
            updateWithCallbacks(mouseInTrigger);
        }
        return false;
    }
}
}