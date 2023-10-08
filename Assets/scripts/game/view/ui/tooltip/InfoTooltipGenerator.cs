using game.view.util;
using UnityEngine;
using util.lang;

namespace game.view.ui.tooltip {
// creates info tooltips
public class InfoTooltipGenerator : Singleton<InfoTooltipGenerator> {

    public InfoTooltipHandler generate(InfoTooltipData data) {
        if (data.type == "entity") {
            if (data.entityType == "item") {
                GameObject tooltip = PrefabLoader.create("itemTooltip");
                tooltip.GetComponent<ItemTooltipHandler>().init(data.entity);    
                return tooltip.GetComponent<InfoTooltipHandler>();
            }
        }
        return null;
    }
}
}