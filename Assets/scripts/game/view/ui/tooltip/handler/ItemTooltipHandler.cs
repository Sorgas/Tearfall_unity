using game.model.component.item;
using game.view.ui.tooltip.handler;
using Leopotam.Ecs;
using TMPro;
using util.lang.extension;

namespace game.view.ui.tooltip {
// special handler for tooltips of items
public class ItemTooltipHandler : DestroyingTooltipHandler {
    private EcsEntity item;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI mainText;
    
    public override void init(InfoTooltipData data) {
        base.init(data);
        item = data.entity;
        titleText.text = item.name();
        string text = "Tags: ";
        foreach (var itemTag in item.take<ItemComponent>().tags) {
            text += itemTag + " ";
        }
        mainText.text = text;
    }
}
}