using enums.material;
using game.model;
using game.model.localmap;
using UnityEngine;
using UnityEngine.UI;
using util.geometry;

namespace game.view.no_es {
    // moves selector sprite on screen, update stat text
    public class MouseMovementSystem {
        private readonly RectTransform selectorSprite;
        private readonly LocalMap map;
        private readonly Text text;
        private readonly IntBounds3 bounds;
        private Vector3Int modelPosition;
        private Vector3 target = new Vector3(0, 0, -1);
        private Vector3 cacheTarget;
        private Vector3 selectorSpeed; // keeps sprite speed between ticks

        public MouseMovementSystem(LocalGameRunner initializer) {
            text = initializer.text;
            selectorSprite = initializer.selector;
            map = GameModel.get().localMap;
            bounds = new IntBounds3(0, 0, 0, map.xSize, map.ySize, map.zSize);
        }

        public void update() {
            if (selectorSprite.localPosition != target)
                selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, target, ref selectorSpeed, 0.05f); // move selector
            updateText();
        }

        public void setTarget(Vector3 value) {
            int z = GameView.get().currentZ;
            modelPosition.Set((int)value.x, (int)(value.y - z / 2f), z); // get model position by scene position
            modelPosition = bounds.putInto(modelPosition);
            target.Set(modelPosition.x, modelPosition.y + z / 2f, -z * 2f - 0.1f); // get scene position by model)
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