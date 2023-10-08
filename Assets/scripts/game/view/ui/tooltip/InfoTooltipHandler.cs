using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.tooltip {
public class InfoTooltipHandler : MonoBehaviour {
    private EcsEntity targetEntity;
    public List<InfoTooltipTrigger> triggers = new();
    public InfoTooltipHandler child;
    private RectTransform self;

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }

    // custom update, called from root tooltip trigger
    public void update() {
        if (child != null) {
            child.update(); // can close child
        }
        if (child == null) {
            Vector3 localPosition = self.InverseTransformPoint(Input.mousePosition);
            if (self.rect.Contains(localPosition)) {
                foreach (var trigger in triggers) {
                    trigger.update(); // check if mouse over triggers
                }
            } else {
                Destroy(gameObject);
            }
        }
    }

    private void moveTooltipToMouse() {
        gameObject.transform.localPosition = Input.mousePosition;
    }
}
}