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
    private LocalMapFollowingCameraController followingController;

    private List<DelayedConditionController> controllers = new List<DelayedConditionController>();
    private List<DelayedConditionController> cameraControllers = new List<DelayedConditionController>();
    private List<DelayedConditionController> selectorControllers = new List<DelayedConditionController>();
    private const int borderPanWidth = 10;

    public LocalMapCameraSystem(Camera camera, RectTransform selector, RectTransform mapHolder) {
        this.camera = camera;
        int xSize = GameModel.get().localMap.xSize;
        int ySize = GameModel.get().localMap.ySize;
        cameraController = new LocalMapCameraController(camera, xSize, ySize);
        selectorController = new LocalMapSelectorController(camera, selector, mapHolder, xSize, ySize);
        followingController = new LocalMapFollowingCameraController(camera, selector);
        initControllers();
    }

    private void initControllers() {
        controllers.Add(new DelayedKeyController(KeyCode.W, () => selectorController.move(0, 1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.A, () => selectorController.move(-1, 0, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.S, () => selectorController.move(0, -1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.D, () => selectorController.move(1, 0, 0)));
        // layers of map are placed with z gap 2 and shifted by y by 0.5
        controllers.Add(new DelayedKeyController(KeyCode.R, () => {
            cameraController.move(0, 0, 1);
            selectorController.move(0, 0, 1);
        })); 
        controllers.Add(new DelayedKeyController(KeyCode.F, () => {
            cameraController.move(0, 0, -1);
            selectorController.move(0, 0, -1);
        }));
        // move camera when mouse on screen border
        cameraControllers.Add(new DelayedConditionController(() => cameraController.move(0, 1, 0), () => (Input.mousePosition.y > Screen.height - borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.move(0, -1, 0), () => (Input.mousePosition.y < borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.move(1, 0, 0), () => (Input.mousePosition.x > Screen.width - borderPanWidth)));
        cameraControllers.Add(new DelayedConditionController(() => cameraController.move(-1, 0, 0), () => (Input.mousePosition.x < borderPanWidth)));
        
        // move camera when mouse on screen border
        // selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(0, 1, 0), () => (Input.mousePosition.y > Screen.height - borderPanWidth)));
        // selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(0, -1, 0), () => (Input.mousePosition.y < borderPanWidth)));
        // selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(1, 0, 0), () => (Input.mousePosition.x > Screen.width - borderPanWidth)));
        // selectorControllers.Add(new DelayedConditionController(() => selectorController.moveTarget(-1, 0, 0), () => (Input.mousePosition.x < borderPanWidth)));
    }

    public void update() {
        float deltaTime = Time.deltaTime;
        if (!enabled) return;
        controllers.ForEach(controller => controller.update(deltaTime));
        cameraController.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        cameraController.update();
        selectorController.update();
        // followingController.update();
    }
}
