﻿using enums.material;
using game.model;
using game.model.localmap;
using game.view.util;
using UnityEngine;
using UnityEngine.UI;
using util.geometry;

namespace game.view.camera {
    // moves selector sprite on screen, update stat text
    public class MouseMovementSystem {
        private readonly RectTransform sprite;
        private readonly LocalMap map;
        private readonly Text text;
        private readonly IntBounds3 bounds;
        private Vector3Int modelPosition;
        private Vector3 target = new Vector3(0, 0, -1);
        private Vector3 cacheTarget;
        private Vector3 speed; // keeps sprite speed between ticks

        public MouseMovementSystem(LocalGameRunner initializer) {
            text = initializer.text;
            sprite = initializer.selector;
            map = GameModel.localMap;
            bounds = new IntBounds3(0, 0, 0, map.xSize, map.ySize, map.zSize);
        }

        public void update() {
            if (sprite.localPosition == target) return;
            sprite.localPosition = Vector3.SmoothDamp(sprite.localPosition, target, ref speed, 0.05f); // move selector
            updateText();
        }

        public void setTarget(Vector3 value) {
            int z = GameView.get().currentZ;
            Vector3Int vector = new Vector3Int((int)value.x, (int)(value.y - z / 2f), z);
            setTargetModel(vector);
        }

        public void setTargetModel(Vector3Int value) {
            modelPosition.Set(value.x, value.y, value.z);
            modelPosition = bounds.putInto(modelPosition);
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f); // get scene position by model
        }

        public Vector3Int getTarget() {
            return modelPosition;
        }

        private void updateText() {
            if (cacheTarget == modelPosition) return;
            var pos = modelPosition;
            if (!map.inMap(pos)) return;
            text.text = "[" + pos.x + ",  " + pos.y + ",  " + pos.z + "]" + "\n"
                        + map.blockType.getEnumValue(pos).NAME + " " + MaterialMap.get().material(map.blockType.getMaterial(pos)).name;
            cacheTarget = modelPosition;
        }
    }
}