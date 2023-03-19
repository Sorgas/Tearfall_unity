using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.util {
    public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public TooltipParentHandler parent;
        
        public void OnPointerEnter(PointerEventData eventData) {
            parent.mouseInTooltip = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            parent.mouseExitedTooltip();
        }
    }
}