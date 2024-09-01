using game.model.component.item;
using Leopotam.Ecs;
using TMPro;
using util.lang.extension;

namespace game.view.ui.tooltip.handler {
// special handler for tooltips of items
public class ItemTooltipHandler : AbstractTooltipHandler {
    protected EcsEntity item;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI qualityText;
    public TextMeshProUGUI valueText; // weight + value
    
    
    public override void init(InfoTooltipData data) {
        base.init(data);
        item = data.entity;
        titleText.text = item.name();
        if (item.Has<ItemQualityComponent>()) {
            qualityText.text = item.take<ItemQualityComponent>().quality.name;
        } else {
            qualityText.text = "";
        }
        ItemComponent itemComponent = item.take<ItemComponent>();
        valueText.text = $"{itemComponent.weight}kg, {itemComponent.value}g";
        // consider adding tags?
    }
}
}