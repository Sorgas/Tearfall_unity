using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.util {
// handles tooltip objects linked to this object. On init, adds TooltipHandler component to tooltip objects
// tooltips should be adjacent to current object
public class TooltipParentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public List<GameObject> tooltips = new(); // to be filled from editor
    private Dictionary<GameObject, bool> tooltipMap = new(); // tooltip -> enabled, contains only populated tooltips
    private bool inited;
    private bool debug = false;

    public void Start() {
        tooltips.ForEach(tooltip => addTooltipObject(tooltip, true));
        if (mouseOverTooltipParent()) showEnabledTooltips();
    }

    public void addTooltipObject(GameObject tooltip, bool enabled) {
        if (!tooltipMap.ContainsKey(tooltip)) {
            tooltipMap.Add(tooltip, enabled); // enabled by default
            TooltipHandler handler = tooltip.AddComponent<TooltipHandler>();
            handler.setParent(this);
        }
        tooltipMap[tooltip] = enabled; // enablement can change
        if (!enabled) tooltip.GetComponent<TooltipHandler>().hide();
    }

    // called from tooltips
    public void mouseExitedTooltip(TooltipHandler handler) {
        closeUnhoveredTooltips();
    }

    // show enabled tooltips
    public void OnPointerEnter(PointerEventData eventData) {
        log("in tooltipParent");
        showEnabledTooltips();
    }

    public void OnPointerExit(PointerEventData eventData) {
        log("out tooltipParent");
        closeUnhoveredTooltips();
    }

    private void showEnabledTooltips() {
        tooltipMap
            .Where(pair => pair.Value)
            .ForEach(pair => pair.Key.GetComponent<TooltipHandler>().show());
    }

    // closes all active tooltips not hovered by mouse
    private void closeUnhoveredTooltips() {
        List<GameObject> raycastedObjects = raycastOnMouse();
        tooltipMap
            .Where(pair => !raycastedObjects.Contains(pair.Key) || !pair.Value) // not hovered or disabled
            .ForEach(pair => pair.Key.GetComponent<TooltipHandler>().hide());
    }

    private bool mouseOverTooltipParent() => raycastOnMouse().Count(go => go == gameObject) > 0;

    private List<GameObject> raycastOnMouse() {
        PointerEventData eventData = new(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Select(result => result.gameObject).ToList();
    }

    private void log(string message) {
        if (debug) Debug.Log(message);
    }
}
}