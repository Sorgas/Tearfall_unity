using game.model;
using game.model.localmap;
using types;
using types.material;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class DebugTileMouseTool : MouseTool {
        private BlockType blockType;
        private int material;

        public void set(string blockType, string material) {
            this.blockType = BlockTypes.get(blockType);
            this.material = MaterialMap.get().id(material);
        }
        
        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            LocalModel model = GameModel.get().currentLocalModel;
            bounds.iterate((x, y, z) => {
                model.localMap.blockType.set(x, y, z, blockType, material);
            });
        }

        public override void updateSprite() {
        }

        public override void rotate() { }

        public override void updateSpriteColor(Vector3Int position) { }

        public override void reset() { }
    }
}