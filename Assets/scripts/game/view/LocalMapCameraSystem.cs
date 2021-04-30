using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.util.geometry;
using Assets.scripts.util.input;
using Tearfall_unity.Assets.scripts.game.view;
using UnityEngine;

// system for controlling camera on local map;
// on key presses camera speed is updated
// on update() camera is moved with it's speed
public class LocalMapCameraInputSystem {
    public Camera camera;
    public bool enabled = true;
    private LocalMapCameraController controller;
    private List<DelayedConditionController> controllers = new List<DelayedConditionController>();
    private IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);
    private const int borderPanWidth = 10;

    public LocalMapCameraInputSystem(Camera camera, RectTransform selector, RectTransform mapHolder) {
        this.camera = camera;
        int xSize = GameModel.get().localMap.xSize;
        int ySize = GameModel.get().localMap.ySize;
        controller = new LocalMapCameraController(camera, selector, mapHolder);
        initControllers();
    }

    public void init() {
        controller.setCameraPosition(GameModel.get().selector.position);
    }

    private void initControllers() {
        controllers.Add(new DelayedKeyController(KeyCode.W, () => controller.moveSelector(0, 1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.A, () => controller.moveSelector(-1, 0, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.S, () => controller.moveSelector(0, -1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.D, () => controller.moveSelector(1, 0, 0)));
        // layers of map are placed with z gap 2 and shifted by y by 0.5
        controllers.Add(new DelayedKeyController(KeyCode.R, () => controller.moveSelector(0, 0, 1))); 
        controllers.Add(new DelayedKeyController(KeyCode.F, () => controller.moveSelector(0, 0, -1)));
        // move camera when mouse on screen border
        controllers.Add(new DelayedConditionController(() => controller.moveCamera(0, 1), () => (Input.mousePosition.y > Screen.height - borderPanWidth)));
        controllers.Add(new DelayedConditionController(() => controller.moveCamera(0, -1), () => (Input.mousePosition.y < borderPanWidth)));
        controllers.Add(new DelayedConditionController(() => controller.moveCamera(1, 0), () => (Input.mousePosition.x > Screen.width - borderPanWidth)));
        controllers.Add(new DelayedConditionController(() => controller.moveCamera(-1, 0), () => (Input.mousePosition.x < borderPanWidth)));
    }

    public void update() {
        if (!enabled) return;
        float deltaTime = Time.deltaTime;
        controllers.ForEach(controller => controller.update(deltaTime));
        controller.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        if (screenBounds.isIn(Input.mousePosition)) { // mouse inside screen
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) controller.resetSelectorToMousePosition(); // update selector position if mouse moved
        }
        controller.update();
    }
}
