﻿using game.view.ui.util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.material_selector {
    // TODO replace with TooltipHandler. Use TooltipParentHandler instead of MaterialButtonHandler
    public class MaterialTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ICloseable {
        public MaterialButtonHandler buttonHandler;

        public void OnPointerEnter(PointerEventData eventData) => buttonHandler.mouseInTooltip = true;

        public void OnPointerExit(PointerEventData eventData) => buttonHandler.mouseExitedTooltip();

        public void close() {
            Debug.Log("closing tooltip");
            gameObject.SetActive(false);
        }

        public void open() {
            Debug.Log("opening tooltip");
            gameObject.SetActive(true);
        }
    }
}