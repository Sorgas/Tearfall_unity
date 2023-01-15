using game.view.ui.util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.material_selector {
    public class MaterialTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ICloseable {
        public MaterialButtonHandler buttonHandler;

        public void OnPointerEnter(PointerEventData eventData) => buttonHandler.mouseInTooltip = true;

        public void OnPointerExit(PointerEventData eventData) => buttonHandler.mouseExitedTooltip();

        public void close() {
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }
    }
}