using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace game.view.ui.util {
// can open tooltip. tracks mouse, and closes tooltip if mouse leaves this object or tooltip.
public class TooltipParentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public List<GameObject> tooltips = new();
    public GameObject openingButton; 
    public bool mouseInParent;
    public bool mouseInTooltip;
    private bool debug = false;
    
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
        if (!mouseInParent && !mouseInTooltip) {
            tooltips.ForEach(tooltip => tooltip.SetActive(false));;
            if(openingButton != null) openingButton.GetComponent<Image>().color = UiColorsEnum.BUTTON_NORMAL;
        }
    }

    private void log(string message) {
        if(debug) Debug.Log(message);
    }
}
}