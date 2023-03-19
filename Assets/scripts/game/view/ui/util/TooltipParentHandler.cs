using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.util {
    
    // can open tooltip. tracks mouse, and closes tooltip if mouse leaves this object or tooltip.
    public class TooltipParentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public GameObject tooltip;
        public bool mouseInParent;
        public bool mouseInTooltip;

        public void Start() {
            tooltip.SetActive(false);
            TooltipHandler tooltipHandler = tooltip.AddComponent<TooltipHandler>();
            tooltipHandler.parent = this;
        }

        public void showTooltip() {
            tooltip.SetActive(true);
        }
        
        public void mouseExitedTooltip() {
            mouseInTooltip = false;
            checkTooltipClosing();
        }

        public void OnPointerEnter(PointerEventData eventData) => mouseInParent= true;

        public void OnPointerExit(PointerEventData eventData) {
            mouseInParent = false;
            checkTooltipClosing();
        }

        private void checkTooltipClosing() {
            if (!mouseInParent && !mouseInTooltip) {
                tooltip.SetActive(false);
            }
        }
    }
}