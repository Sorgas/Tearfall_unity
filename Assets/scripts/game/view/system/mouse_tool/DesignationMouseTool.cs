using game.model;
using game.view.camera;
using game.view.util;
using types;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class DesignationMouseTool : MouseTool {
        public DesignationType designation;

        public DesignationMouseTool() {
            selectionType = SelectionType.AREA;
            name = "designation mouse tool";
        }

        public override void onSelectionInToolbar() {
            materialSelector.close();
            prioritySelector.open();
            prioritySelector.setForTool(this);
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            if (designation == null) Debug.LogError("designation is null");
            addUpdateEvent(model => {
                bounds.iterate((x, y, z) => {
                    Vector3Int position = new(x, y, z);
                    // Debug.Log("designation tool tile " + bounds.toString());
                    if (designation == DesignationTypes.D_CLEAR) {
                        // TODO should cancel designation
                        model.designationContainer.removeDesignation(position);
                    } else {
                        if (designation.validator.validate(position, model)) {
                            model.designationContainer.createDesignation(position, designation, priority);
                        }
                    }
                });
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
            return designation.validator == null || designation.validator.validate(position, GameModel.get().currentLocalModel);
        }

        public override void rotate() { }
    }
}