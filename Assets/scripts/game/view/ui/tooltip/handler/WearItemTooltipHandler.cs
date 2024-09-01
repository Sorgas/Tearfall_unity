using game.model.component.item;
using game.model.system;
using game.view.util;
using TMPro;
using types.item;
using UnityEngine.UI;
using util;
using util.lang.extension;

namespace game.view.ui.tooltip.handler {
public class WearItemTooltipHandler : ItemTooltipHandler {
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI insulationText;
    public TextMeshProUGUI slotText;

    public override void init(InfoTooltipData newData) {
        base.init(newData);
        
        ItemWearComponent wearComponent = item.take<ItemWearComponent>();
        defenceText.text = $"{wearComponent.defence}";
        insulationText.text = $"Insulation {wearComponent.insulation}";
        slotText.text = $"{wearComponent.slot} {wearComponent.layer}";
    }
}
}