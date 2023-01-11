using game.model.component.item;
using Leopotam.Ecs;
using types.item.type;
using UnityEngine;
using util.lang.extension;

namespace game.view.util {
    public class ItemVisualUtil {

        public static Sprite resolveItemSprite(EcsEntity item) {
            ItemComponent component = item.take<ItemComponent>();
            ItemType type = ItemTypeMap.getItemType(component.type);
            if (item.Has<ItemVisualComponent>()) {
                return item.take<ItemVisualComponent>().sprite;
            } else {
                return ItemTypeMap.get().getSprite(type.name);
            }
        }
    }
}