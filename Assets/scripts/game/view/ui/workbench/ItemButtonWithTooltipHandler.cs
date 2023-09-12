using System;
using System.Linq;
using game.model.component.item;
using game.view.ui.util;
using Leopotam.Ecs;
using TMPro;
using types.item;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.workbench {
// Displays item icon. On mouse hover, can show tooltip.
// TODO make tooltips to be related to mouse pointer
[RequireComponent(typeof(TooltipParentHandler))]
public class ItemButtonWithTooltipHandler : MonoBehaviour {
    // button
    public Image background;
    public Image image; // item image
    public TextMeshProUGUI quantityText;

    // tooltip
    public bool tooltipEnabled;
    public RectTransform tooltip;
    public TextMeshProUGUI tooltipTitle;
    public TextMeshProUGUI tooltipText;

    public virtual void initFor(EcsEntity item, int amount = -1) {
        bool haveItem = item != EcsEntity.Null;
        if (haveItem) showItem(item.take<ItemComponent>(), amount);
        image.gameObject.SetActive(haveItem);
        gameObject.GetComponent<TooltipParentHandler>().addTooltipObject(tooltip.gameObject, haveItem);
    }

    // sets icon, fills tooltip, sets quantity text
    private void showItem(ItemComponent item, int amount) {
        string typeName = item.type;
        Material_ material = MaterialMap.get().material(item.material);
        image.sprite = ItemTypeMap.get().getSprite(typeName);
        image.color = MaterialMap.get().material(item.material).color;
        // TODO add item stats
        tooltipTitle.text = material.name + " " + typeName;
        tooltipText.text = item.tags // TODO add item description as main text
            .Select(tag => ItemTags.findTag(tag))
            .Where(tag => tag.displayable)
            .Select(tag => tag.displayName)
            .Aggregate((tag1, tag2) => tag1 + ", " + tag2);
        Vector2 size = new Vector2(Math.Max(tooltipTitle.preferredWidth, tooltipText.preferredWidth) + 10,
            tooltipTitle.preferredHeight + tooltipTitle.preferredHeight + 15);
        tooltip.sizeDelta = size;
        quantityText.text = amount < 0 ? "" : $"{amount}";
    }
}
}