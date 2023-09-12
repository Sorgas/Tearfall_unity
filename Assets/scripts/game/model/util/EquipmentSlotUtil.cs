using System;
using System.Linq;
using game.model.component.item;
using game.model.component.unit;
using Leopotam.Ecs;
using util;
using util.lang.extension;

namespace game.model.util {
// helper class for operations on items and equipment slots.
// Wearable items have layer and slot property. Wear slots can contain one item of each slot.
// Grabbing slots(hands) can contain one item.
// performs validations and throws exceptions.
public static class EquipmentSlotUtil {
    public static readonly string[] validLayers = { "wear", "armor", "over" };

    public static EcsEntity getItemInSlot(WearEquipmentSlot slot, string layer) {
        validateLayer(layer);
        return layer switch {
            "wear" => slot.item,
            "armor" => slot.armorItem,
            "over" => slot.overItem,
        };
    }

    public static void equipItemToSlot(WearEquipmentSlot slot, EcsEntity item) {
        string layer = item.take<ItemWearComponent>().layer;
        validateLayer(layer);
        if (getItemInSlot(slot, layer) != EcsEntity.Null) {
            throw new GameException($"setting item {item.name()} while another item equipped");
        }
        switch (layer) {
            case "wear":
                slot.item = item;
                break;
            case "armor":
                slot.armorItem = item;
                break;
            case "over":
                slot.overItem = item;
                break;
        }
    }

    public static EcsEntity removeItemFromSlot(WearEquipmentSlot slot, string layer) {
        validateLayer(layer);
        EcsEntity item = EcsEntity.Null;
        if (layer == "wear") {
            item = slot.item;
            slot.item = EcsEntity.Null;
        } else if (layer == "armor") {
            item = slot.overItem;
            slot.armorItem = EcsEntity.Null;
        } else if (layer == "over") {
            item = slot.overItem;
            slot.overItem = EcsEntity.Null;
        }
        return item;
    }

    private static void validateLayer(string layer) {
        if(!validLayers.Contains(layer)) throw new ArgumentException($"{layer} is invalid value for item layer", layer, null);
    }
}
}