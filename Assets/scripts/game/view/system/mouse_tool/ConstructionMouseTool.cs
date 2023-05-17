using game.model;
using game.model.container;
using game.model.util.validation;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionType;
using DesignationContainer = game.model.container.DesignationContainer;

namespace game.view.system.mouse_tool {
    public class ConstructionMouseTool : ItemConsumingMouseTool {
        public ConstructionType type;
        private ConstructionValidator validator = new();

        public override void onSelectionInToolbar() {
            fillSelectorForVariants(type.name, type.variants);
            prioritySelector.open();
            prioritySelector.init(this);
        }

        public void set(ConstructionType type) {
            this.type = type;
            selectionType = type.name == "wall" ? ROW : AREA;
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            if (!hasMaterials) {
                Debug.LogWarning("no materials for construction");
                return;
            }
            addUpdateEvent(model => {
                bounds.iterate((x, y, z) => {
                    Vector3Int position = new(x, y, z);
                    model.designationContainer.createConstructionDesignation(position, type, itemType, material);
                });
            });
        }

        public override void updateSprite() {
            selectorGO.setConstructionSprite(selectSpriteByBlockType());
        }

        public override void rotate() { }

        public override void updateSpriteColor(Vector3Int position) {
            selectorGO.buildingValid(validate(position));
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setConstructionSprite(null);
            selectionType = AREA;
        }

        public bool validate(Vector3Int position) {
            return validator.validateForConstruction(position.x, position.y, position.z, type, GameModel.get().currentLocalModel);
        }

        private Sprite selectSpriteByBlockType() {
            if (visualMaterial == null) {

            }
            Debug.Log("getting material variant " + visualMaterial);
            return BlockTileSetHolder.get().getSprite(visualMaterial,
                type.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : type.blockType.PREFIX);
        }
    }
}