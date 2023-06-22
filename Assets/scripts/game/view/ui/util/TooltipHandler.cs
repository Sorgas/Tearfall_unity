using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.util {
    public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private TooltipParentHandler parent;
        private bool debug = false;
        
        public void OnPointerEnter(PointerEventData eventData) {
            log("in tooltip");
            parent.mouseInTooltip = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            log("out tooltip");
            parent.mouseExitedTooltip();
        }

        public void setParent(TooltipParentHandler parent) {
            this.parent = parent;
        }

        private void log(string message) {
            if(debug) Debug.Log(message);
        }
    }
}