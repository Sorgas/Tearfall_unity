using game.model;
using game.view.camera;
using game.view.util;
using types;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class DesignationMouseTool : MouseTool {
        public DesignationType designation;
        private string iconPath;
        
        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void updateSelectionType(bool materialsOk) {
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.AREA; // set default type
        }

        public override void applyTool(IntBounds3 bounds) {
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                if (designation.validator.validate(position)) {
                    GameModel.get().designationContainer.createDesignation(position, designation);
                }
            });
        }

        public override void updateSprite(bool materialsOk) {
            selector.setToolSprite(IconLoader.get(iconPath));
        }
    }
}