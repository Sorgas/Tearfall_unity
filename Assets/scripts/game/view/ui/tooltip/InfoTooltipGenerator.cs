using System.Collections.Generic;
using game.model.component.item;
using game.view.ui.tooltip.handler;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.view.ui.tooltip {
// selects tooltip prefab,
// creates info tooltips
public class InfoTooltipGenerator : Singleton<InfoTooltipGenerator> {
    private Dictionary<string, InfoTooltipData> textTooltipDefinitions;

    public AbstractTooltipHandler generate(InfoTooltipData data) {
        if (data.type == "entity") {
            if (data.entityType == "item") {
                string tooltipName = "itemTooltip";
                if (data.entity.Has<ItemWeaponComponent>()) {
                    tooltipName = "WeaponItemTooltip";
                } else if (data.entity.Has<ItemWearComponent>()) {
                    tooltipName = "WearItemTooltip";
                }
                GameObject go = PrefabLoader.create(tooltipName);
                AbstractTooltipHandler handler = go.GetComponent<AbstractTooltipHandler>();
                handler.init(data);
                return handler;
            }
        }
        if (data.type == "dummy") {
            GameObject tooltip = PrefabLoader.create("dummyTooltip");
            return tooltip.GetComponent<AbstractTooltipHandler>();
        }
        if (data.type == "disease") {
            GameObject tooltip = PrefabLoader.create("unitDiseaseTooltip");
            DiseaseTooltipHandler handler = tooltip.GetComponent<DiseaseTooltipHandler>();
            handler.init(data);
            return handler;
        }
        return null;
    }

    public AbstractTooltipHandler generateFromLink(string link) {
        // TODO reformat text
        GameObject tooltip = PrefabLoader.create("textTooltip");
        tooltip.GetComponent<TextTooltipHandler>().init(getDataForTextTooltip(link));
        return tooltip.GetComponent<AbstractTooltipHandler>();
    }

    private InfoTooltipData getDataForTextTooltip(string name) {
        if (textTooltipDefinitions == null) initTextTooltipDefinitions();
        return textTooltipDefinitions[name];
    }

    private void initTextTooltipDefinitions() {
        textTooltipDefinitions = new();
        textTooltipDefinitions.Add("dummyRecursive", new InfoTooltipData("toolbar/rotate", "Dummy Tooltip", 
            "This is first dummy tooltip with circular <link=\"dummyRecursive2\">link</link> to another dummy tooltip."));
        textTooltipDefinitions.Add("dummyRecursive2", new InfoTooltipData("toolbar/rotate", "Dummy Tooltip", 
            "This is second dummy tooltip with circular <link=\"dummyRecursive\">link</link> to another dummy tooltip."));
    }

    private void formatTooltipText(string text) {
        int lb = text.IndexOf('[');
        int rb = text.IndexOf(']');
        int length = rb - lb + 1; // brackets included
        string linkData = text.Substring(lb + 1, length - 2);
        string[] args = linkData.Split(":");
        text.Remove(lb, length);
    }
}
}