using UnityEngine;

namespace game.view.ui.tooltip.handler {
// deactivates tooltip go when mouse leaves
public class HidingTooltipHandler : AbstractTooltipHandler {
 
    protected override void handleMouseLeave() {
        gameObject.SetActive(false);
        Debug.Log("tooltip hidden");
    }
}
}