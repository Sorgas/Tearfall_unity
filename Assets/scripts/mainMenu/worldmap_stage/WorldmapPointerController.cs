using UnityEngine;
using util.geometry.bounds;
using Vector3 = UnityEngine.Vector3;

namespace mainMenu.worldmap_stage {
// Moves pointer around world map. Updates target position for camera.
// TODO when moving mouse, text hint should update by translated mouse position
// TODO when clicking mouse, pointer should move to translated mouse position
public class WorldmapPointerController {
    public Transform pointer;
    public Transform locationSelector;

    private IntBounds2 bounds; // allowed pointer positions, set to world size
    private PlayerControls playerControls;
    private RectTransform worldViewport;
    private Camera worldCamera;
    private WorldmapController controller;

    public WorldmapPointerController(WorldmapController controller) {
        this.controller = controller;
        pointer = controller.pointer;
        locationSelector = controller.locationSelector;
        playerControls = controller.playerControls;
        worldViewport = controller.mask;
        worldCamera = controller._camera;
        int worldSize = controller.worldMap.size;
        bounds = new IntBounds2(0, 0, worldSize - 1, worldSize - 1);
        controller.mapPanel.onClick.AddListener(handleClick);
    }

    public void update() {
        Vector2 mousePosition = playerControls.UI.Point.ReadValue<Vector2>();
        Vector3Int worldPosition = getWorldPositionByScreenPosition(mousePosition);
        Vector3 pointerPosition = bounds.putInto(worldPosition);
        if (pointer.localPosition != pointerPosition) {
            pointer.localPosition = pointerPosition;
            controller.locationChanged = true;
        }
    }

    private Vector3Int getWorldPositionByScreenPosition(Vector2 screenPos) {
        Vector2 viewportPosition; // canvas units
        RectTransformUtility.ScreenPointToLocalPointInRectangle(worldViewport, screenPos, null, out viewportPosition);
        float cameraH = worldCamera.orthographicSize; // map units
        float cameraW = worldCamera.aspect * cameraH; // map units
        Vector2 onMapPosition = (cameraW * 2 / worldViewport.rect.width) * viewportPosition; // map units
        var cameraLocalPosition = worldCamera.transform.localPosition;
        onMapPosition.x += cameraLocalPosition.x;
        onMapPosition.y += cameraLocalPosition.y;
        return new Vector3Int((int)onMapPosition.x, (int)onMapPosition.y, -1);
    }
    
    private void handleClick() {
        Debug.Log("clicked");
        if (!locationSelector.gameObject.activeSelf) return;
        locationSelector.localPosition = pointer.localPosition;
        controller.locationChanged = true;
    }
}
}