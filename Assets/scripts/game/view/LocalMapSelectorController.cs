using Assets.scripts.util.geometry;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    // moves tile selector on local map. Selector is instantly moved in game model. 
    // Selector sprite is smoothly moved to in-model selector position.
    public class LocalMapSelectorController {
        public RectTransform selector;
        public RectTransform mapHolder;
        public Camera camera;
        private IntVector3 position = new IntVector3();
        private IntBounds3 bounds = new IntBounds3(); // whole map bounds
        private Vector3 target = new Vector3(0,0,-1);
        private Vector3 speed = new Vector3();
        IntVector3 cacheVector = new IntVector3();
        public bool enabled = true;

        public LocalMapSelectorController(Camera camera, RectTransform selector, RectTransform mapHolder, int mapSize, int mapLayers) {
            this.selector = selector;
            this.camera = camera;
            this.mapHolder = mapHolder;
            bounds.set(0, 0, 0, mapSize, mapSize, mapLayers);
        }

        // smoothly moves camera towards cameraTarget
        public void update() {
            if (!enabled) return;
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) handleMouseMovement();
            target.Set(position.x, position.y + position.z/2f, -2 * position.z - 0.1f); // update target by in-model position
            selector.localPosition = Vector3.SmoothDamp(selector.localPosition, target, ref speed, 0.05f); // smooth movement
        }

        // moves model position of selector and visual movement target
        public void move(int dx, int dy, int dz) {
            if (!enabled) return;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            position.add(dx, dy, dz); // update model position of selector
            ensureBounds();
        }

        // reset selector to mouse position on current level
        private void handleMouseMovement() {
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition);
            position.set((int)mapHolderPosition.x, -position.z / 2f + mapHolderPosition.y, position.z); // update model position of selector
            ensureBounds();
        }

        private void ensureBounds() {
            if (!bounds.isIn(position)) position.add(bounds.getInVector(position)); // return selector into map, if needed
        }
    }
}