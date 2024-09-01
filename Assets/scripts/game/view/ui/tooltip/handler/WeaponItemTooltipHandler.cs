using game.model.component.item;
using game.model.system;
using game.view.util;
using TMPro;
using types.item;
using UnityEngine.UI;
using util;
using util.lang.extension;

namespace game.view.ui.tooltip.handler {
public class WeaponItemTooltipHandler : ItemTooltipHandler {
    public Image damageTypeIcon; // TODO make icon a tooltipTrigger for damageType tooltip
    public TextMeshProUGUI damageText; // + dpm TODO make dps a tooltipTrigger for per in-game minute explanation
    public TextMeshProUGUI speedText; // + accuracy
    public TextMeshProUGUI gripTypeText; // TODO make tooltipTrigger for grip explanation

    public override void init(InfoTooltipData newData) {
        base.init(newData);
        ItemWeaponComponent weaponComponent = item.take<ItemWeaponComponent>();
        setDamageTypeIcon(weaponComponent.damageType);
        float dpm = weaponComponent.damage * weaponComponent.accuracy * (GameTime.ticksPerMinute / weaponComponent.reload);
        damageText.text = $"{weaponComponent.damageType} {weaponComponent.damage:##} dpm:{dpm:##.##}";
        speedText.text = $"{weaponComponent.reload}, accuracy: {weaponComponent.accuracy}";
        gripTypeText.text = $"{selectGripTypeString(item.take<ItemGripComponent>().type)} {weaponComponent.skill} weapon";
    }

    private string selectGripTypeString(string gripType) {
        switch (gripType) {
            case "main": return "One-handed";
            case "off": return "Off-hand";
            case "two": return "Two-handed";
            case "any": return "One-handed";
            default: throw new GameException($"invalid grip type {gripType}");
        }
    }

    private void setDamageTypeIcon(string damageType) {
        DamageType type = DamageTypes.get(damageType);
        damageTypeIcon.sprite = IconLoader.get().getSprite("damage_types/" + type.iconName);
        damageTypeIcon.color = type.color;
    }
}
}