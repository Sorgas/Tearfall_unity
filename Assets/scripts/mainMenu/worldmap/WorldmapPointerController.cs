using System.Collections;
using System.Collections.Generic;
using Assets.scripts.util.geometry;
using Assets.scripts.util.input;
using UnityEngine;

namespace Assets.scripts.mainMenu.worldmap {
    // Moves pointer around world map. Updates target position for camera.
    public class WorldmapPointerController {
        public Vector2 targetPosition = new Vector2Int(); // target position for worldmap camera
        public Transform pointer;
        public bool pointerMoved = false;

        private IntBounds2 bounds; // allowed pointer positions
        private IntBounds2 targetBounds; // allowed target positions
        private List<DelayedKeyController> controllers = new List<DelayedKeyController>();
        private Vector2 cachePosition = new Vector2();

        public WorldmapPointerController(int worldSize, Transform pointer) {
            this.pointer = pointer;
            bounds = new IntBounds2(0, 0, worldSize - 1, worldSize - 1);
            targetBounds = new IntBounds2(bounds).extend(2);
            controllers.Add((new DelayedKeyController(KeyCode.UpArrow, () => movePointer(0, 1))));
            controllers.Add((new DelayedKeyController(KeyCode.DownArrow, () => movePointer(0, -1))));
            controllers.Add((new DelayedKeyController(KeyCode.LeftArrow, () => movePointer(-1, 0))));
            controllers.Add((new DelayedKeyController(KeyCode.RightArrow, () => movePointer(1, 0))));
        }

        public void update() {
            pointerMoved = false;
            controllers.ForEach(controller => controller.update(Time.deltaTime));
        }

        private void movePointer(int dx, int dy) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                dx *= 10;
                dy *= 10;
            }
            cachePosition.Set(pointer.localPosition.x + dx, pointer.localPosition.y + dy); // player moves pointer
            pointer.localPosition = bounds.putInto(cachePosition); // limit pointer by bounds
            targetPosition = pointer.localPosition;
            pointerMoved = true;
            if (!pointer.localPosition.Equals(cachePosition)) {
                if (cachePosition.x < bounds.minX) targetPosition.x = targetBounds.minX;
                if (cachePosition.x > bounds.maxX) targetPosition.x = targetBounds.maxX;
                if (cachePosition.y < bounds.minY) targetPosition.y = targetBounds.minY;
                if (cachePosition.y > bounds.maxY) targetPosition.y = targetBounds.maxY;
            }
        }
    }
}