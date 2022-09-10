using game.model;
using game.model.container;
using game.model.util.validation;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionTypes;

namespace game.view.system.mouse_tool {
    public class ConstructionMouseTool : ItemConsumingMouseTool {
        public ConstructionType type;
        private ConstructionValidator validator = new();
        
        public override bool updateMaterialSelector() {
            return fillSelectorForVariants(type.variants);
        }

        public void set(ConstructionType type) {
            this.type = type;
            selectionType = type.name == "wall" ? ROW : AREA;
        }

        public override void applyTool(IntBounds3 bounds) {
            DesignationContainer container = GameModel.get().designationContainer;
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                container.createConstructionDesignation(position, type, itemType, material);
            });
        }

        public override void updateSprite() {
            selectorGO.setConstructionSprite(selectSpriteByBlockType());
        }

        public override void rotate() {}

        public override void updateSpriteColor(Vector3Int position) {
            selectorGO.buildingValid(validate(position));
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setConstructionSprite(null);
            selectionType = AREA;
        }

        public bool validate(Vector3Int position) {
            return validator.validateForConstruction(position.x, position.y, position.z, type);
        }

        private Sprite selectSpriteByBlockType() {
            Debug.Log("getting material varian " + visualMaterial);
            return BlockTileSetHolder.get().getSprite(visualMaterial,
                type.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : type.blockType.PREFIX);
        }
    }
}