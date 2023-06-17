using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.util {
// can open tooltip. tracks mouse, and closes tooltip if mouse leaves this object or tooltip.
public class TooltipParentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public List<GameObject> tooltips = new();
    public bool mouseInParent;
    public bool mouseInTooltip;
    private bool debug = true;
    
    public void Start() {
        tooltips.ForEach(tooltip => {
            tooltip.SetActive(false);
            TooltipHandler tooltipHandler = tooltip.AddComponent<TooltipHandler>();
            tooltipHandler.setParent(this);
        });
    }

    public void mouseExitedTooltip() {
        mouseInTooltip = false;
        checkTooltipClosing();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        log("in tooltipParent");
        mouseInParent = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        log("out tooltipParent");
        mouseInParent = false;
        checkTooltipClosing();
    }

    private void checkTooltipClosing() {
        if (!mouseInParent && !mouseInTooltip) enableTooltips(false);
    }

    private void enableTooltips(bool value) => tooltips.ForEach(tooltip => tooltip.SetActive(value));
    
    private void log(string message) {
        if(debug) Debug.Log(message);
    }
}
}