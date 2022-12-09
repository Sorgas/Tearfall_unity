using System;
using System.Collections.Generic;
using enums.item.type;
using game.model.component.building;
using game.view.system.mouse_tool;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

public class WorkbenchInventoryHandler : MonoBehaviour {
    private EcsEntity entity;
    public RectTransform scrollContent;

    public void toggle() => gameObject.SetActive(!gameObject.activeSelf);

    public void hide() => gameObject.SetActive(false);

    public void initFor(EcsEntity entity) {
        this.entity = entity;
        foreach(Transform child in scrollContent) {
            GameObject.Destroy(child.gameObject);
        }
        if (entity.Has<BuildingItemContainerComponent>()) {
            List<EcsEntity> items = entity.take<BuildingItemContainerComponent>().items;
            for (var i = 0; i < items.Count; i++) {
                GameObject panel = PrefabLoader.create("itemPanelWithTooltip", scrollContent);
                panel.transform.localPosition = getPanelPosition(panel.GetComponent<RectTransform>(), i);
                ItemPanelWithTooltip tooltipHandler = panel.GetComponent<ItemPanelWithTooltip>();
                tooltipHandler.initFor(items[i], 1);
            }
        }
    }

    private Vector3 getPanelPosition(RectTransform panel, int i) {
        int rowSize = (int)Math.Floor(scrollContent.rect.width / panel.rect.width);
        int row = i / rowSize;
        int positionInRow = i % rowSize;
        return new Vector3(positionInRow * panel.rect.width, - row * panel.rect.height, 0);
    }
}