namespace game.view.ui.tooltip.handler {
// deactivates tooltip go when mouse leaves
public class HidingTooltipHandler : AbstractTooltipHandler {
    protected override void closeTooltip() {
        gameObject.SetActive(false);
        parent = null; // unlink trigger
    }
}
}