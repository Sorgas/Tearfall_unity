using game.model;
using game.model.localmap;
using game.view.ui;
using game.view.util;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.geometry.bounds;

namespace game.view.camera {
    // moves selector sprite on screen, updates stat text
    // updates selector sprite basing on tile
    public class MouseMovementSystem {
        private readonly RectTransform selector;
        private readonly LocalMap map;
        private readonly Text debugLabelText;
        private readonly IntBounds3 bounds;
        private SelectorSpriteUpdater spriteUpdater;
        private Vector3Int modelPosition;
        private Vector3 target = new(0, 0, -1);
        private Vector3 cacheTarget;
        private Vector3 speed; // keeps sprite speed between ticks

        public MouseMovementSystem(LocalGameRunner initializer) {
            debugLabelText = initializer.debugInfoPanel;
            selector = initializer.selector;
            map = GameModel.localMap;
            bounds = new IntBounds3(0, 0, 0, map.bounds.maxX, map.bounds.maxY, map.bounds.maxZ);
            spriteUpdater = new SelectorSpriteUpdater(map, selector.GetComponent<SelectorHandler>());
        }

        public void update() {
            if (selector.localPosition == target) return;
            selector.localPosition = Vector3.SmoothDamp(selector.localPosition, target, ref speed, 0.05f); // move selector
            if (cacheTarget == modelPosition) return;
            updateText();
            spriteUpdater.updateSprite(modelPosition);
            cacheTarget = modelPosition;
        }

        public void setTarget(Vector3 value) {
            int z = GameView.get().currentZ;
            Vector3Int vector = new((int)value.x, (int)(value.y - z / 2f), z);
            setTargetModel(vector);
        }

        public void setTargetModel(Vector3Int value) {
            modelPosition.Set(value.x, value.y, value.z);
            modelPosition = bounds.putInto(modelPosition);
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition); // get scene position by model
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f);
        }

        public Vector3Int getTarget() {
            return modelPosition;
        }

        private void updateText() {
            if (!map.inMap(modelPosition)) return;
            debugLabelText.text = "coord: [" + modelPosition.x + ",  " + modelPosition.y + ",  " + modelPosition.z + "]" + "\n"
                                  + "block: " + map.blockType.getEnumValue(modelPosition).NAME + " " + MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name + "\n"
                                  + "passage area: " + map.passageMap.area.get(modelPosition);
        }
    }
}