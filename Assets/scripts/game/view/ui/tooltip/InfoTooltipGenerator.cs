using System.Collections.Generic;
using game.view.ui.tooltip.handler;
using game.view.util;
using UnityEngine;
using util.lang;

namespace game.view.ui.tooltip {
// selects tooltip prefab,
// creates info tooltips
public class InfoTooltipGenerator : Singleton<InfoTooltipGenerator> {
    private Dictionary<string, InfoTooltipData> textTooltipDefinitions;

    public DestroyingTooltipHandler generate(InfoTooltipData data) {
        if (data.type == "entity") {
            if (data.entityType == "item") {
                GameObject go = PrefabLoader.create("itemTooltip");
                DestroyingTooltipHandler handler = go.GetComponent<DestroyingTooltipHandler>();
                handler.init(data);
                return handler;
            }
        }
        if (data.type == "dummy") {
            GameObject tooltip = PrefabLoader.create("dummyTooltip");
            return tooltip.GetComponent<DestroyingTooltipHandler>();
        }
        return null;
    }

    public DestroyingTooltipHandler generateFromLink(string link) {
        // TODO reformat text
        GameObject tooltip = PrefabLoader.create("textTooltip");
        tooltip.GetComponent<TextTooltipHandler>().init(getDataForTextTooltip(link));
        return tooltip.GetComponent<DestroyingTooltipHandler>();
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