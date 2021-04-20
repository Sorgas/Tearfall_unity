using Assets.scripts.game.model;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    // moves tile selector on local map. Selector is instantly moved in game model. 
    // Selector sprite is smoothly moved to in-model selector position.
    public class LocalMapSelectorController {
        public RectTransform selector;
        public int mapSize;
        public int mapLayers;

        private IntVector3 logicalTarget = new IntVector3();
        private IntBounds3 bounds = new IntBounds3(); // bounds for logical target
        private Vector3 target = new Vector3();


        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private Vector3 speed = new Vector3();
        private FloatBounds2 effectiveCameraSize = new FloatBounds2(); // number of visible tiles, relative to camera position
        private FloatBounds2 visibleArea = new FloatBounds2();

        private Vector3 maxSpeedScaleVector = new Vector3(2, 2, 2);
        private Vector3 speedScale = new Vector3(0.3f, 0.3f, 0.3f);

        public LocalMapSelectorController(RectTransform selector, int mapSize, int mapLayers) {
            this.selector = selector;
            this.mapSize = mapSize;
            this.mapLayers = mapLayers;
            bounds.set(0, 0, 0, mapSize, mapSize, mapLayers);
        }

        // smoothly moves camera towards cameraTarget
        public void update() {
            selector.localPosition = Vector3.SmoothDamp(selector.localPosition, target, ref speed, 0.2f);
        }

        public void moveTarget(int dx, int dy, int dz) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust speed for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            logicalTarget.x += dx;
            logicalTarget.y += dy;
            logicalTarget.z += dz;
            // GameModel.get().
        
            ensureTargetInBounds();
            translateTarget();
        }

        private void ensureTargetInBounds() {
            if (!bounds.isIn(logicalTarget)) logicalTarget.add(bounds.getInVector(logicalTarget));
        }

        public void translateTarget() {
            target.Set(logicalTarget.x, logicalTarget.y + logicalTarget.z * 0.5f, -2 * logicalTarget.z - 0.1f);
        }

        public void setSpritePosition(int x, float y, int z) {
            selector.Translate(x, y, z, Space.World);
        }
    }
}