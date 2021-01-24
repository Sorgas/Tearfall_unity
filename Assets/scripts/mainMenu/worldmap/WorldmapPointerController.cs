
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util.geometry;

public class WorldmapPointerController {
    private IntBounds2 bounds;
    private Transform pointer;
    private List<DelayedKeyController> controllers = new List<DelayedKeyController>();


    public WorldmapPointerController(int worldSize, Transform pointer) {
        this.pointer = pointer;
        bounds = new IntBounds2(0,0, worldSize, worldSize);
        controllers.Add((new DelayedKeyController(KeyCode.UpArrow, () => movePointer(0, 1))));
        controllers.Add((new DelayedKeyController(KeyCode.DownArrow, () => movePointer(0, -1))));
        controllers.Add((new DelayedKeyController(KeyCode.LeftArrow, () => movePointer(-1, 0))));
        controllers.Add((new DelayedKeyController(KeyCode.RightArrow, () => movePointer(1, 0))));
    }

    public void update() {
        controllers.ForEach(controller => controller.update(Time.deltaTime));
    }

    private void movePointer(int dx, int dy) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) {
            dx *= 10;
            dy *= 10;
        }
        Vector3 position = pointer.localPosition;
        if (position.x + dx < bounds.minX) dx = (int)(bounds.minX - position.x);
        if (position.x + dx > bounds.maxX) dx = (int)(bounds.maxX - position.x);
        if (position.y + dy < bounds.minY) dy = (int)(bounds.minY - position.y);
        if (position.y + dy > bounds.maxY) dy = (int)(bounds.maxY - position.y);
        pointer.Translate(dx, dy, 0);
    }
}
