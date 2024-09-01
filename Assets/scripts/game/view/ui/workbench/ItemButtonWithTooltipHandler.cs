using game.model.component.item;
using game.view.ui.tooltip;
using game.view.ui.tooltip.producer;
using game.view.ui.tooltip.trigger;
using Leopotam.Ecs;
using TMPro;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.workbench {
// Displays item icon. When item set, initializes tooltip trigger.
[RequireComponent(typeof(AbstractTooltipTrigger))]
[RequireComponent(typeof(AbstractTooltipProducer))]
public class ItemButtonWithTooltipHandler : MonoBehaviour {
    protected EcsEntity item;
    // button
    public Image background; // TODO change background by item quality
    public Image image; // item image
    public TextMeshProUGUI quantityText;

    private AbstractTooltipTrigger tooltipTrigger;
    private AbstractTooltipProducer tooltipProducer;

    public void Awake() {
        tooltipTrigger = GetComponent<AbstractTooltipTrigger>();
        tooltipProducer = GetComponent<AbstractTooltipProducer>();
    }

    public void initFor(EcsEntity item, int amount = -1) {
        this.item = item;
        bool haveItem = item != EcsEntity.Null;
        if (haveItem) showItem(item, amount);
        image.gameObject.SetActive(haveItem);
        tooltipTrigger.enabled = haveItem;
    }

    // sets icon, fills tooltip, sets quantity text
    private void showItem(EcsEntity entity, int amount) {
        ItemComponent item = entity.take<ItemComponent>();
        string typeName = item.type;
        tooltipProducer.setData(new InfoTooltipData(entity, "item"));
        image.sprite = ItemTypeMap.get().getSprite(typeName);
        image.color = MaterialMap.get().material(item.material).color;
        quantityText.text = amount < 0 ? "" : $"{amount}";
    }
}
}