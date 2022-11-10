using System;
using System.Linq;
using enums.item;
using enums.item.type;
using game.model.component.item;
using Leopotam.Ecs;
using TMPro;
using types.material;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using util.lang.extension;

class ItemPanelWithTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private EcsEntity item;
    public Image image;
    public TextMeshProUGUI text;

    public GameObject tooltip;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemMaterial;
    public TextMeshProUGUI itemTags;

    public void initFor(EcsEntity item, int amount) {
        this.item = item;
        ItemComponent itemComponent = item.take<ItemComponent>();
        string typeName = itemComponent.type;
        Material_ material = MaterialMap.get().material(itemComponent.material);
        image.sprite = ItemTypeMap.get().getSprite(typeName);
        text.text = material.name + " " + typeName + " " + amount;

        itemName.text = material.name + " " + typeName;
        itemMaterial.text = material.name; // TODO add description
        itemTags.text = itemComponent.tags
            .Select(tag => Enum.GetName(typeof(ItemTagEnum), tag))
            .Aggregate((tag1, tag2) => tag1 + ", " + tag2);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }
}