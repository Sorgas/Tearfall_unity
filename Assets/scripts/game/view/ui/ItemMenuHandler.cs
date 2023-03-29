using System;
using System.Linq;
using game.input;
using game.model.component.item;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.item;
using types.item.type;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui {
    public class ItemMenuHandler : MbWindow, IHotKeyAcceptor {
        public const string name = "item_menu";
        public Image image;
        public TextMeshProUGUI itemTitle;
        public TextMeshProUGUI itemMaterial;
        public TextMeshProUGUI itemTags;
    
        public EcsEntity item;

        public bool accept(KeyCode key) {
            if(key == KeyCode.Q) WindowManager.get().closeWindow(name);
            return true;
        }

        public void FillForItem(EcsEntity item) {
            ItemComponent component = item.take<ItemComponent>();
            ItemType type = ItemTypeMap.getItemType(component.type);
            itemTitle.text = type.name;
            itemMaterial.text = component.materialString;
            itemTags.text = component.tags.Aggregate((tag1, tag2) => tag1 + ", " + tag2);
            image.sprite = ItemVisualUtil.resolveItemSprite(item);
        }

        public override string getName() {
            return name;
        }
    }
}