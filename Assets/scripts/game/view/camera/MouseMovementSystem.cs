using game.model;
using game.model.localmap;
using game.view.ui;
using game.view.util;
using types.material;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.camera {
    // stores selector position
    // moves selector sprite on screen, updates stat text
    // updates selector sprite basing on tile
    public class MouseMovementSystem {
        private readonly RectTransform selector;
        private readonly LocalMap map;
        private readonly Text debugLabelText;
        private readonly SelectorSpriteUpdater spriteUpdater;
        private Vector3 target = new(0, 0, -1); // target for sprite GO in scene coords
        private Vector3 cacheTarget; // to avoid excess GO moving
        private Vector3 speed; // keeps sprite speed between ticks

        public MouseMovementSystem(LocalGameRunner initializer) {
            debugLabelText = initializer.debugInfoPanel;
            selector = initializer.selector;
            map = GameModel.localMap;
            spriteUpdater = new SelectorSpriteUpdater(map, selector.GetComponent<SelectorHandler>());
        }

        public void update() {
            if (selector.localPosition == target) return;
            selector.localPosition = Vector3.SmoothDamp(selector.localPosition, target, ref speed, 0.05f); // move selector
            Vector3Int modelPosition = GameView.get().selectorPosition;
            if (cacheTarget == modelPosition) return;
            updateText();
            spriteUpdater.updateSprite(modelPosition);
            cacheTarget = modelPosition;
        }

        // update selector position and sprite target (called from MIS)
        public void setTarget(Vector3 value) {
            Vector3Int modelPosition = ViewUtil.fromSceneToModelInt(value);
            GameView.get().setSelectorPosition(modelPosition);
            setTargetModel(modelPosition);
        }

        // update target without updating position in model (called from GV)
        public void setTargetModel(Vector3Int value) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(value);
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f);
        }

        private void updateText() {
            Vector3Int modelPosition = GameView.get().selectorPosition;
            if (!map.inMap(modelPosition)) return;
            debugLabelText.text = "coord: [" + modelPosition.x + ",  " + modelPosition.y + ",  " + modelPosition.z + "]" + "\n"
                                  + "block: " + map.blockType.getEnumValue(modelPosition).NAME + " " +
                                  MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name + "\n"
                                  + "passage area: " + map.passageMap.area.get(modelPosition);
        }
    }
}