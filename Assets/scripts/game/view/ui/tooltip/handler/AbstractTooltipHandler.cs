using System.Collections.Generic;
using System.Linq;
using game.view.ui.tooltip.trigger;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.tooltip {
// Provides basic functionality of tooltip: initializing, tooltip frame checking (for closing), calling updates on triggers.
public abstract class AbstractTooltipHandler : MonoBehaviour {
    private InfoTooltipData data;
    private EcsEntity targetEntity;
    protected List<AbstractTooltipTrigger> triggers = new();
    private RectTransform self;

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
        triggers = new List<AbstractTooltipTrigger>(GetComponentsInChildren<AbstractTooltipTrigger>());
    }

    public virtual void init(InfoTooltipData data) {
        this.data = data;
    }
    
    // custom update, called from root tooltip trigger
    // keep self - should tooltip not close itself, even if mouse is outside
    public void update(bool keepSelf) {
        foreach (var trigger in triggers) {
            trigger.update(); // can open tooltip or pass update further
        }
        if (!hasChild()) { // no child tooltip, can close self
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            bool mouseInTooltip = self.rect.Contains(localPosition);
            if (!mouseInTooltip && !keepSelf) { // mouse is outside tooltip and parent trigger
                handleMouseLeave();
            }
        }
    }

    protected abstract void handleMouseLeave();
    
    private bool hasChild() {
        return triggers.Any(trigger => trigger.tooltip != null);
    }
}
}