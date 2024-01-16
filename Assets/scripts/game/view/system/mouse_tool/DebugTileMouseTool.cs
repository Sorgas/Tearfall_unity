using game.view.tilemaps;
using types;
using types.material;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    // tool for placing blocks on map
    public class DebugTileMouseTool : MouseTool {
        private BlockType blockType;
        private int material;

        public DebugTileMouseTool() {
            name = "debug tile mouse tool";
        }

        public void set(string blockType, string material) {
            this.blockType = BlockTypes.get(blockType);
            this.material = MaterialMap.get().id(material);
        }

        public override void onSelectionInToolbar() {
            materialSelector.close();
            prioritySelector.close();
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            addUpdateEvent(model => {
                bounds.iterate(pos => {
                    model.localMap.blockType.set(pos, blockType, material);
                });
            });
        }

        public override void updateSprite() {
            Sprite sprite = BlockTileSetHolder.get().getSprite("template", blockType.CODE == BlockTypes.RAMP.CODE ? "C" : blockType.PREFIX);
            selectorGO.setConstructionSprite(sprite);
        }

        public override void rotate() { }

        public override void updateSpriteColor(Vector3Int position) { }

        public override void reset() { }
    }
}