using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.tooltip.trigger {
// Opens tooltip when trigger clicked
[RequireComponent(typeof(Button))]
public abstract class ClickingTooltipTrigger : AbstractTooltipTrigger {
    public override void Awake() {
        base.Awake();
        gameObject.GetComponent<Button>().onClick.AddListener(openTooltip);
    }

    protected override bool openCondition() {
        return false;
    }
}
}