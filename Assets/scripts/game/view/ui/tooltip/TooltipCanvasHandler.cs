using UnityEngine;

namespace game.view.ui.tooltip {
public class TooltipCanvasHandler : MonoBehaviour {
    private bool forceOccupied;

    public void Start() {
        forceOccupied = false;
    }
    
    public bool isOccupied() {
        return forceOccupied || transform.childCount > 0;
    }

    public void occupy() {
        forceOccupied = true;
    }

    public void free() {
        forceOccupied = false;
    }
}
}