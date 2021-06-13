using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using Assets.scripts.util.input;
using Tearfall_unity.Assets.scripts.game;
using Tearfall_unity.Assets.scripts.game.model.entity_selector;
using Tearfall_unity.Assets.scripts.game.view;
using UnityEngine;
using UnityEngine.UI;
using Tearfall_unity.Assets.scripts.enums.material;

// system for controlling camera on local map;
// has controllers for handling single key press or single mouse position condition. on update, calls all controllers.
// on key presses camera speed is updated
// on update() camera is moved with it's speed
public class LocalMapCameraInputSystem {
    public Camera camera;
    public bool enabled = true;
    private LocalMap localMap;
    private EntitySelector selector = GameModel.get().selector;
    private LocalMapCameraController controller;
    private List<DelayedConditionController> controllers = new List<DelayedConditionController>();
    private IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);
    private Text text;

    public LocalMapCameraInputSystem(LocalGameRunner initializer) {
        this.camera = initializer.mainCamera;
        localMap = GameModel.get().localMap;
        int xSize = localMap.xSize;
        int ySize = localMap.ySize;
        this.text = initializer.text;
        screenBounds.extendX((int)(-Screen.width * 0.01f));
        screenBounds.extendY((int)(-Screen.height * 0.01f));
        controller = new LocalMapCameraController(camera, initializer.selector, initializer.mapHolder);

        initControllers();
    }

    public void init() {
        controller.setCameraPosition(GameModel.get().selector.position);
    }

    private void initControllers() {
        controllers.Add(new DelayedKeyController(KeyCode.W, () => handleWASD(0, 1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.A, () => handleWASD(-1, 0, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.S, () => handleWASD(0, -1, 0)));
        controllers.Add(new DelayedKeyController(KeyCode.D, () => handleWASD(1, 0, 0)));
        // layers of map are placed with z gap 2 and shifted by y by 0.5
        controllers.Add(new DelayedKeyController(KeyCode.R, () => controller.moveSelector(0, 0, 1)));
        controllers.Add(new DelayedKeyController(KeyCode.F, () => controller.moveSelector(0, 0, -1)));
        // move camera when mouse on screen border
        controllers.Add(new DelayedConditionController(() => handleScreenBorder(0, 1), () => (Input.mousePosition.y > Screen.height * 0.99f)));
        controllers.Add(new DelayedConditionController(() => handleScreenBorder(0, -1), () => (Input.mousePosition.y < Screen.height * 0.01f)));
        controllers.Add(new DelayedConditionController(() => handleScreenBorder(1, 0), () => (Input.mousePosition.x > Screen.width * 0.99f)));
        controllers.Add(new DelayedConditionController(() => handleScreenBorder(-1, 0), () => (Input.mousePosition.x < Screen.width * 0.01f)));
    }

    public void update() {
        if (!enabled) return;
        float deltaTime = Time.deltaTime;
        Vector3Int currentPosition = selector.position;
        controllers.ForEach(controller => controller.update(deltaTime));
        controller.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        if (screenBounds.isIn(Input.mousePosition)) { // mouse inside screen
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) controller.resetSelectorToMousePosition(Input.mousePosition); // update selector position if mouse moved
        }
        controller.update();
        if (selector.position != currentPosition) updateText();
    }

    private void handleWASD(int dx, int dy, int dz) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
            dx *= 10;
            dy *= 10;
        }
        Vector3Int delta = controller.moveSelector(dx, dy, dz);
        dx -= delta.x;
        dy -= delta.y;
        controller.moveCameraTarget(dx, dy);
    }

    private void handleScreenBorder(int dx, int dy) {
        controller.moveCameraTarget(dx, dy);
        controller.moveSelector(dx, dy, 0);
    }

    private void updateText() {
        text.text = "[" + selector.position.x + ",  " + selector.position.y + ",  " + selector.position.z + "]" + "\n"
        + localMap.blockType.getEnumValue(selector.position).NAME + " " + MaterialMap.get().material(localMap.blockType.getMaterial(selector.position)).name;
    }
}
