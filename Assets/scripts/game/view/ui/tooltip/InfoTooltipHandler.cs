using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.tooltip {
// handler for tooltip. 
// tooltip calls updates on all child tooltip triggers and can close itself.
public class InfoTooltipHandler : MonoBehaviour {
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
        Debug.Log($"udpating Tooltip, trigger count: {triggers.Count}");
        foreach (var trigger in triggers) {
            trigger.update(); // can open tooltip or pass update further
        }
        if (!hasChild()) { // no child tooltip, can close self
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            bool mouseInTooltip = self.rect.Contains(localPosition);
            if (!mouseInTooltip && !keepSelf) { // mouse is outside tooltip and parent trigger
                Destroy(gameObject);
            }
        }
    }

    private bool hasChild() {
        return triggers.Any(trigger => trigger.tooltip != null);
    }
}
}