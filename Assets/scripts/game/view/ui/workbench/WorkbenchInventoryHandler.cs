using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.building;
using game.model.component.item;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.workbench {
    // shows icons for items stored in workbench
    // TODO consider using list of rows instead of square buttons
    public class WorkbenchInventoryHandler : MonoBehaviour {
        private EcsEntity entity;
        public RectTransform scrollContent;

        public void toggle() => gameObject.SetActive(!gameObject.activeSelf);

        public void hide() => gameObject.SetActive(false);

        public void initFor(EcsEntity entity) {
            this.entity = entity;
            foreach(Transform child in scrollContent) {
                Destroy(child.gameObject);
            }
            if (entity.Has<BuildingItemContainerComponent>()) {
                int count = 0;
                Dictionary<ItemComponent, int> items = groupItems(entity.take<BuildingItemContainerComponent>().items);
                foreach (var pair in items) {
                    GameObject panel = PrefabLoader.create("itemIconWithTooltip", scrollContent);
                    panel.transform.localPosition = getPanelPosition(panel.GetComponent<RectTransform>(), count);
                    ItemPanelWithTooltip tooltipHandler = panel.GetComponent<ItemPanelWithTooltip>();
                    tooltipHandler.initFor(pair.Key, pair.Value);
                    count++;
                }
            }
        }

        // groups items by itemType and material
        private Dictionary<ItemComponent, int> groupItems(List<EcsEntity> items) {
            return items.Select(item => item.take<ItemComponent>())
                .GroupBy(component => component.type + component.material)
                .ToDictionary(grouping => grouping.First(), grouping => grouping.Count());
        }
        
        private Vector3 getPanelPosition(RectTransform panel, int i) {
            Debug.Log(scrollContent.rect.width + " _ " + panel.rect.width);
            int rowSize = (int)Math.Floor(scrollContent.rect.width / panel.rect.width); // how many icons fit into scroll content
            int row = i / rowSize;
            int positionInRow = i % rowSize;
            return new Vector3(positionInRow * panel.rect.width, - row * panel.rect.height, 0);
        }
    }
}