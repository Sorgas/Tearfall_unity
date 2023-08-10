using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using TMPro;
using types.item;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.view.ui.workbench {
// inits button for item. shows and hides tooltip on mouse hover    
public class ItemButtonWithTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // button
    public Image background;
    public Image image;
    public TextMeshProUGUI quantityText;

    // tooltip
    public RectTransform tooltip;
    public TextMeshProUGUI title;
    public TextMeshProUGUI text;

    public void Start() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        bool isOverButton = results.Count(result => result.gameObject == background.gameObject) > 0;
        tooltip.gameObject.SetActive(isOverButton);
    }
    
    public void initFor(ItemComponent item, int amount) {
        string typeName = item.type;
        Material_ material = MaterialMap.get().material(item.material);
        image.sprite = ItemTypeMap.get().getSprite(typeName);
        image.color = MaterialMap.get().material(item.material).color;
        quantityText.text = amount + "";

        // TODO add item stats
        title.text = material.name + " " + typeName;
        text.text = item.tags // TODO add item description as main text
            .Select(tag => ItemTags.findTag(tag))
            .Where(tag => tag.displayable)
            .Select(tag => tag.displayName)
            .Aggregate((tag1, tag2) => tag1 + ", " + tag2);
        Vector2 size = new Vector2(Math.Max(title.preferredWidth, text.preferredWidth) + 10,
            title.preferredHeight + title.preferredHeight + 15);
        tooltip.sizeDelta = size;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.gameObject.SetActive(false);
    }
}
}