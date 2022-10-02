using game.model;
using game.model.localmap;
using game.view.util;
using types.material;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.camera {
    // moves selector sprite on screen, updates stat text
    // updates selector sprite basing on tile
    public class MouseMovementSystem {
        private readonly RectTransform selectorSprite;
        private Vector3 target = new(0, 0, -1); // target for sprite GO in scene coords
        private Vector3Int modelTarget;
        private Vector3Int cacheTarget; // to avoid excess GO moving
        private Vector3 speed; // keeps sprite speed between ticks
        // for debug only TODO decompose
        private readonly LocalMap map;
        private readonly Text debugLabelText;

        public MouseMovementSystem(LocalGameRunner initializer) {
            debugLabelText = initializer.debugInfoPanel;
            selectorSprite = initializer.selector;
            map = GameModel.localMap;
        }

        public void update() {
            // move selector towards target
            updateText(modelTarget);
            if (selectorSprite.localPosition == target) return;
            selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, target, ref speed, 0.05f); // move selector
            
            if (cacheTarget == modelTarget) return;
            cacheTarget = modelTarget;
        }

        // update target when mouse moved (called from MIS), only way to update targets
        public void updateTarget(Vector3Int modelPosition) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f);
            modelTarget = modelPosition;
        }

        // update target and sprite position immediately (called from GV)
        public void updateTargetAndSprite(Vector3Int modelPosition) {
            updateTarget(modelPosition);
            selectorSprite.localPosition.Set(target.x, target.y, target.y);
        }

        private void updateText(Vector3Int modelPosition) {
            if (!map.inMap(modelPosition)) return;
            debugLabelText.text = "coord: [" + modelPosition.x + ",  " + modelPosition.y + ",  " + modelPosition.z + "]" + "\n"
                                  + "block: " + map.blockType.getEnumValue(modelPosition).NAME + " " +
                                  MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name + "\n"
                                  + "passage area: " + map.passageMap.area.get(modelPosition) + "\n"
                                  + "UPS: " + GameModel.get().counter.lastUPS; 
        }
    }
}