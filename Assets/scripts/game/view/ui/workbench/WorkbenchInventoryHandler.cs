using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.workbench {
    // shows icons for items stored in workbench
    public class WorkbenchInventoryHandler : MonoBehaviour {
        private EcsEntity entity;
        public RectTransform scrollContent;

        public void toggle() => gameObject.SetActive(!gameObject.activeSelf);

        public void hide() => gameObject.SetActive(false);

        public void initFor(EcsEntity entity) {
            this.entity = entity;
            clear();
            redraw();
        }

        public void Update() {
            if (!entity.take<ItemContainerComponent>().updated) return;
            clear();
            redraw();
            entity.takeRef<ItemContainerComponent>().setUpdated(false);
        }

        private void clear() {
            foreach(Transform child in scrollContent) {
                Destroy(child.gameObject);
            }
        }
        
        private void redraw() {
            if (entity.Has<ItemContainerComponent>()) {
                int count = 0;
                Dictionary<EcsEntity, int> items = groupItems(entity.take<ItemContainerComponent>().items);
                foreach (var pair in items) {
                    GameObject panel = PrefabLoader.create("itemButtonWithTooltip", scrollContent);
                    panel.transform.localPosition = getPanelPosition(panel.GetComponent<RectTransform>(), count);
                    ItemButtonWithTooltipHandler tooltipHandlerHandler = panel.GetComponent<ItemButtonWithTooltipHandler>();
                    tooltipHandlerHandler.initFor(pair.Key, pair.Value);
                    count++;
                }
            }
        }
        
        // groups items by itemType and material
        private Dictionary<EcsEntity, int> groupItems(List<EcsEntity> items) {
            return items
                .GroupBy(item => {
                    ItemComponent component = item.take<ItemComponent>();
                    return component.type + component.material;
                })
                .ToDictionary(grouping => grouping.First(), grouping => grouping.Count());
        }
        
        private Vector3 getPanelPosition(RectTransform panel, int i) {
            int rowSize = (int)Math.Floor(scrollContent.rect.width / panel.rect.width); // how many icons fit into scroll content
            int row = i / rowSize;
            int positionInRow = i % rowSize;
            return new Vector3(positionInRow * panel.rect.width, - row * panel.rect.height, 0);
        }
    }
}