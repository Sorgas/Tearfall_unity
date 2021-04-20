using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.util.input;
using Tearfall_unity.Assets.scripts.game.view;
using UnityEngine;

// system for controlling camera on local map;
// on key presses camera speed is updated
// on update() camera is moved with it's speed
public class LocalMapCameraSystem {
    public Camera camera;
    public bool enabled = true;
    private LocalMapCameraController cameraController;
    private LocalMapSelectorController selectorController;

    private List<DelayedConditionController> controllers = new List<DelayedConditionController>();
    private List<DelayedConditionController> cameraControllers = new List<DelayedConditionController>();
    private List<DelayedConditionController> selectorControllers = new List<DelayedConditionController>();
    private const int borderPanWidth = 10;

    public LocalMapCameraSystem(Camera camera, RectTransform selector) {
        this.camera = camera;
        cameraController = new LocalMapCameraController(camera, GameModel.get().localMap.xSize, GameModel.get().localMap.xSize);
        selectorController = new LocalMapSelectorController(selector, GameModel.get().localMap.xSize, GameModel.get().localMap.xSize);
        initControllers();
    }

    private void initControllers() {
        cameraControllers.Add(new DelayedKeyController(KeyCode.W, () => cameraController.moveTarget(0, 1, 0)));
        cameraControllers.Add(new DelayedKeyController(KeyCode.A, () => cameraController.moveTarget(-1, 0, 0)));
        cameraControllers.Add(new DelayedKeyController(KeyCode.S, () => cameraController.moveTarget(0, -1, 0)));
        cameraControllers.Add(new DelayedKeyController(KeyCode.D, () => cameraController.moveTarget(1, 0, 0)));
        cameraControllers.Add(new DelayedKeyController(KeyCode.R, () => cameraController.moveTarget(0, 0, 1))); // layers of map are placed with z gap 2 and shifted by y by 0.5
        cameraControllers.Add(new DelayedKeyController(KeyCode.F, () => cameraController.moveTarget(0, 0, -1)));
        // move camera when mouse on screen border
        cameraControllers.Add(new DelayedConditionController(() => cameraController.moveTarget(0, 1, 0), () => (Input.mousePosition.y > Screen.height - borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.moveTarget(0, -1, 0), () => (Input.mousePosition.y < borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.moveTarget(1, 0, 0), () => (Input.mousePosition.x > Screen.width - borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.moveTarget(-1, 0, 0), () => (Input.mousePosition.x < borderPanWidth)));
        selectorControllers.Add(new DelayedKeyController(KeyCode.W, () => selectorController.moveTarget(0, 1, 0)));
        selectorControllers.Add(new DelayedKeyController(KeyCode.A, () => selectorController.moveTarget(-1, 0, 0)));
        selectorControllers.Add(new DelayedKeyController(KeyCode.S, () => selectorController.moveTarget(0, -1, 0)));
        selectorControllers.Add(new DelayedKeyController(KeyCode.D, () => selectorController.moveTarget(1, 0, 0)));
        selectorControllers.Add(new DelayedKeyController(KeyCode.R, () => selectorController.moveTarget(0, 0, 1))); // layers of map are placed with z gap 2 and shifted by y by 0.5
        selectorControllers.Add(new DelayedKeyController(KeyCode.F, () => selectorController.moveTarget(0, 0, -1)));
        // move camera when mouse on screen border
        selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(0, 1, 0), () => (Input.mousePosition.y > Screen.height - borderPanWidth)));
        selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(0, -1, 0), () => (Input.mousePosition.y < borderPanWidth)));
        selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(1, 0, 0), () => (Input.mousePosition.x > Screen.width - borderPanWidth)));
        selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(-1, 0, 0), () => (Input.mousePosition.x < borderPanWidth)));
        controllers = cameraControllers;
    }

    public void update() {
        float deltaTime = Time.deltaTime;
        if (!enabled) return;
        controllers.ForEach(controller => controller.update(deltaTime));
        cameraController.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        cameraController.update();
    }
}
