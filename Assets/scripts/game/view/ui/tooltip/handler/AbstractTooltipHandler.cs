using System.Collections.Generic;
using System.Linq;
using game.view.ui.tooltip.trigger;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.tooltip.handler {
// Provides basic functionality of tooltip: initializing, tooltip frame checking (for closing), calling updates on triggers.
public abstract class AbstractTooltipHandler : MonoBehaviour {
    private InfoTooltipData data;
    private EcsEntity targetEntity;
    protected List<AbstractTooltipTrigger> triggers = new();
    private RectTransform self;
    public AbstractTooltipTrigger parent; // link to trigger

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
        triggers = new List<AbstractTooltipTrigger>(GetComponentsInChildren<AbstractTooltipTrigger>());
    }

    public virtual void init(InfoTooltipData newData) => data = newData;

    // custom update, called from root tooltip trigger
    // keep self - should tooltip not close itself, even if mouse is outside
    // returns true, if tooltip not closed after update
    public bool update(bool keepSelf) {
        AbstractTooltipTrigger activeTrigger = getActiveTrigger();
        if (activeTrigger == null) { // update triggers to try to open tooltip
            bool hasChild = false;
            foreach (var trigger in triggers) {
                hasChild = trigger.updateInternal();
                if (hasChild) break;
            }
            // close self if have no children, parent trigger not hovered, and self not hovered
            if (!hasChild && !keepSelf &&
                !self.rect.Contains(self.InverseTransformPoint(Input.mousePosition))) {
                handleMouseLeave();
                return false;
            }
        } else {
            return activeTrigger.updateInternal();
        }
        return true;
    }

    protected abstract void closeTooltip();

    // unlink from parent and close self
    private void handleMouseLeave() {
        parent = null;
        closeTooltip();
    }

    private AbstractTooltipTrigger getActiveTrigger() {
        return triggers.FirstOrDefault(trigger => trigger.isTooltipOpen());
    }
}
}