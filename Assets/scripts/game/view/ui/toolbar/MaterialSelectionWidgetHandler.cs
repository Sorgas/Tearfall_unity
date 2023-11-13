using game.model.component.task.order;
using game.view.ui.workbench;
using UnityEngine;

namespace game.view.ui.toolbar {
// Widget for selecting materials for ingredientConsumingOrder
// shows buttons for selecting material for construction or building.
    // TODO keep selected material between usages
    // TODO add multi-ingredient buildings
    public class MaterialSelectionWidgetHandler : MonoBehaviour {
        public CraftingOrderConfigPanelHandler handler; 
        private string currentBuilding;

        public bool fillForOrder(AbstractItemConsumingOrder order) {
            handler.fillFor(order);
            return true;
        }

        // // selects itemType/material combination to mouse tool
        // public void select(string itemType, int materialId) {
        //     MouseToolManager.get().setItem(itemType, materialId);
        //     buttons.ForEach(button => button.updateSelected(itemType, materialId));
        // }

        public void close() => gameObject.SetActive(false);

        public void open() => gameObject.SetActive(true);
    }
}