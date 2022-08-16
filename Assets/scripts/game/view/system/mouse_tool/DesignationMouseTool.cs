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

        public DesignationMouseTool() {
            selectionType = SelectionTypes.AREA;
        }

        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void applyTool(IntBounds3 bounds) {
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                
                if (designation == null) Debug.Log("design is null");
                Debug.Log(designation.name);
                if (designation.validator == null) Debug.Log("validator is null");
                if (designation.validator.validate(position)) {
                    GameModel.get().designationContainer.createDesignation(position, designation);
                }
            });
        }

        public override void updateSprite() {
            selectorGO.setToolSprite(IconLoader.get(designation.iconName));
        }
        
        public override void updateSpriteColor(Vector3Int position) {
            selectorGO.designationValid(validate(position));
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setToolSprite(null);
        }

        private bool validate(Vector3Int position) {
            return designation.validator == null || designation.validator.validate(position);
        }

        public override void rotate() { }
    }
}