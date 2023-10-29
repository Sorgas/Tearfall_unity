namespace game.view.ui.tooltip.handler {

// tooltip, that destroys itself when unhovered
public class DestroyingTooltipHandler : AbstractTooltipHandler {
    
    protected override void closeTooltip() {
        Destroy(gameObject);
    }
}
}