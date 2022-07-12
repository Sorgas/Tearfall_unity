using game.model;
using game.model.container;
using game.view.camera;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class ConstructionMouseTool : ItemConsumingMouseTool {
        public ConstructionType type;
        
        public override bool updateMaterialSelector() {
            return fillSelectorForVariants(type.variants);
        }

        public override void updateSelectionType(bool materialsOk) {
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = type.name == "wall" 
                ? SelectionTypes.ROW
                : SelectionTypes.AREA;
        }

        public override void applyTool(IntBounds3 bounds) {
            DesignationContainer container = GameModel.get().designationContainer;
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                container.createConstructionDesignation(position, type, itemType, material);
            });
        }

        public override void updateSprite(bool materialsOk) {
            selector.setConstructionSprite(selectSpriteByBlockType());
        }

        private Sprite selectSpriteByBlockType() {
            return BlockTileSetHolder.get().getSprite(visualMaterial,
                type.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : type.blockType.PREFIX);
        }
    }
}