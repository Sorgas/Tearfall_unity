using game.model;
using game.model.entity_selector;
using game.model.localmap;
using types.material;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.with_entity_selector {
    // sets selector target to position of model selector.
    // moves selector sprite to selector target.
    // updates hint text
    public class EntitySelectorVisualMovementSystem {
        // common
        private RectTransform selectorSprite;
        // selector
        private Vector3 selectorTarget = new Vector3(0, 0, -1); // target in scene coordinates
        private Vector3 selectorSpeed = new Vector3(); // keeps sprite speed between ticks

        private Text text;
        private Vector3Int selectorPositionCache;
        private LocalMap map;

        Vector3Int cacheVector = new Vector3Int();
    
        public EntitySelectorVisualMovementSystem(LocalGameRunner runner) {
            text = runner.sceneElementsReferences.modelDebugInfoPanel;
            selectorSprite = runner.sceneElementsReferences.selector;
            map = GameModel.get().currentLocalModel.localMap;
        }

        public void update() {
            // var pos = selector.position;
            // selectorTarget.Set(pos.x, pos.y + pos.z / 2f, -2 * pos.z - 0.1f); // update target by in-model position
            // if (selectorSprite.localPosition != selectorTarget)
            //     selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, selectorTarget, ref selectorSpeed, 0.05f); // move selector
            // if(pos != selectorPositionCache) {
            //     updateText();
            //     selectorPositionCache = pos;
            // }
        }

        private void updateText() {
            // var pos = selector.position;
            // text.text = "[" + pos.x + ",  " + pos.y + ",  " + pos.z + "]" + "\n"
            //             + map.blockType.getEnumValue(pos).NAME + " " + MaterialMap.get().material(map.blockType.getMaterial(pos)).name;
        }
    }
}