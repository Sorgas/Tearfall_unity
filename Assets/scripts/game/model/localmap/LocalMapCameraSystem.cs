using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.util.input;
using Tearfall_unity.Assets.scripts.game.view;
using UnityEngine;

// system for controlling camera on local map;
public class LocalMapCameraSystem {
    public Camera camera;
    public bool enabled = true;
    private LocalMapCameraController controller;
    private List<DelayedKeyController> controllers = new List<DelayedKeyController>();
    private const int borderPanWidth = 10;

    public LocalMapCameraSystem(Camera camera) {
        this.camera = camera;
        controller = new LocalMapCameraController(camera, GameModel.get().localMap.xSize, GameModel.get().localMap.xSize);
        controllers.Add(new DelayedKeyController(KeyCode.W, () => controller.moveTarget(0, 1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.A, () => controller.moveTarget(-1, 0, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.S, () => controller.moveTarget(0, -1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.D, () => controller.moveTarget(1, 0, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.R, () => controller.moveTarget(0,0,1))); // layers of map are placed with z gap 2 and shifted by y by 0.5
        controllers.Add(new DelayedKeyController(KeyCode.F, () => controller.moveTarget(0,0,-1)));
    }

    public void update() {
        float deltaTime = Time.deltaTime;
        if (!enabled) return;
        controllers.ForEach(controller => controller.update(deltaTime));
        controller.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        controller.update();
    }

    private void checkScreenBorder(float deltaTime) {
        if (Input.mousePosition.y > Screen.height - borderPanWidth) {

        }
        if (Input.mousePosition.y < borderPanWidth) {

        }
        if (Input.mousePosition.x > Screen.width - borderPanWidth) {

        }
        if (Input.mousePosition.x < borderPanWidth) {

        }
    }
}
