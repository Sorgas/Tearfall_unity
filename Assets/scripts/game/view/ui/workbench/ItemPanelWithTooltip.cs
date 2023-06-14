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
    class ItemPanelWithTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public Image image;
        public TextMeshProUGUI text;
        public TextMeshProUGUI quantityText;

        public GameObject tooltip;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemMaterial;
        public TextMeshProUGUI itemTags;

        public void initFor(ItemComponent item, int amount) {
            string typeName = item.type;
            Material_ material = MaterialMap.get().material(item.material);
            image.sprite = ItemTypeMap.get().getSprite(typeName);
            image.color = MaterialMap.get().material(item.material).color;
            text.text = material.name + " " + typeName + " " + amount;
            quantityText.text = amount + "";
            
            itemName.text = material.name + " " + typeName;
            itemMaterial.text = material.name; // TODO add description
            itemTags.text = item.tags
                .Select(tag => ItemTags.findTag(tag))
                .Where(tag => tag.displayable)
                .Select(tag => tag.displayName)
                .Aggregate((tag1, tag2) => tag1 + ", " + tag2);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            tooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tooltip.SetActive(false);
        }
    }
}