using game.model.component.item;
using Leopotam.Ecs;
using TMPro;
using util.lang.extension;

namespace game.view.ui.tooltip {
public class ItemTooltipHandler : InfoTooltipHandler {
    private EcsEntity item;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI mainText;
    
    public void init(EcsEntity item) {
        this.item = item;
        titleText.text = item.name();
        string text = "";
        foreach (var itemTag in item.take<ItemComponent>().tags) {
            text += itemTag + " ";
        }
        mainText.text = text;
    }
}
}