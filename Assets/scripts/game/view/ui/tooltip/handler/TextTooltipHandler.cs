using game.view.ui.tooltip.handler;
using game.view.util;
using TMPro;
using UnityEngine.UI;

namespace game.view.ui.tooltip {
// handler for general text tooltip
public class TextTooltipHandler : DestroyingTooltipHandler {
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI text;

    public override void init(InfoTooltipData data) {
        base.init(data);
        icon.sprite = IconLoader.get().getSprite(data.icon);
        title.text = data.title;
        text.text = data.text;
    }
}
}