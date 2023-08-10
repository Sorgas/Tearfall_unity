using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using UnityEngine.EventSystems;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.view.ui.util {
public class TooltipParentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public List<GameObject> tooltips = new();
    
    private List<TooltipHandler> handlers = new();
    private bool debug = false;
    
    public void Start() {
        tooltips.ForEach(tooltip => {
            tooltip.SetActive(false);
            TooltipHandler handler = tooltip.AddComponent<TooltipHandler>();
            handler.setParent(this);
            handlers.Add(handler);
        });
    }

    public void mouseExitedTooltip() {
        checkTooltipClosing();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        log("in tooltipParent");
        handlers.ForEach(handler => handler.show());
    }

    public void OnPointerExit(PointerEventData eventData) {
        log("out tooltipParent");
        checkTooltipClosing();
    }

    // closes all active tooltips not hovered by mouse
    private void checkTooltipClosing() {
        PointerEventData eventData = new(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        List<GameObject> raycastedObjects = results
            .Where(result => result.gameObject.hasComponent<TooltipHandler>())
            .Select(result => result.gameObject)
            .Where(go => tooltips.Contains(go))
            .ToList();
        foreach (var handler in handlers) {
            if (raycastedObjects.Contains(handler.gameObject)) {
                handler.show();
            } else {
                handler.hide();
            }
        }
    }

    private void log(string message) {
        if(debug) Debug.Log(message);
    }
}
}