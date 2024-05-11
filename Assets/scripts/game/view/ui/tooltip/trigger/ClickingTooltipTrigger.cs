using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.tooltip.trigger {
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