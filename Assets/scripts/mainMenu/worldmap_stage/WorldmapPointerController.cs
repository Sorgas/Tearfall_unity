using game.model;
using UnityEngine;
using UnityEngine.UI;
using util.geometry.bounds;
using Vector3 = UnityEngine.Vector3;

namespace mainMenu.worldmap_stage {
// Moves pointer around world map. Updates target position for camera.
// TODO when moving mouse, text hint should update by translated mouse position
// TODO when clicking mouse, pointer should move to translated mouse position
public class WorldmapPointerController : MonoBehaviour {
    public Transform pointer;
    public Transform locationSelector;
    public RectTransform viewport;
    public Camera camera;
    public Button mapImage;

    public Vector3Int pointerPosition; 
    private IntBounds2 bounds; // allowed pointer positions, set to world size
    private PlayerControls playerControls;
    private Vector2 previousMousePosition;
    
    public void Start() {
        playerControls = new PlayerControls();
        playerControls.Enable();
        mapImage.onClick.AddListener(handleClick);
    }
    
    public void Update() {
        if (bounds == null) return; 
        Vector2 mousePosition = playerControls.UI.Point.ReadValue<Vector2>();
        if (mousePosition != previousMousePosition) {
            pointerPosition = bounds.putInto(getWorldPositionByScreenPosition(mousePosition));
            if (pointer.localPosition != pointerPosition) {
                pointer.localPosition = pointerPosition;
            }
            previousMousePosition = mousePosition;
        }
    }

    public void setWorldMap(WorldMap worldMap) {
        if (worldMap != null) {
            int worldSize = worldMap.size;
            bounds = new IntBounds2(0, 0, worldSize - 1, worldSize - 1);
        }
    }
    
    // can return position outside game world
    private Vector3Int getWorldPositionByScreenPosition(Vector2 screenPos) {
        Vector2 viewportPosition; // canvas units
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, null, out viewportPosition);
        float cameraH = camera.orthographicSize; // map units
        float cameraW = camera.aspect * cameraH; // map units
        Vector2 onMapPosition = (cameraW * 2 / viewport.rect.width) * viewportPosition; // map units
        var cameraLocalPosition = camera.transform.localPosition;
        onMapPosition.x += cameraLocalPosition.x;
        onMapPosition.y += cameraLocalPosition.y;
        return new Vector3Int((int)onMapPosition.x, (int)onMapPosition.y, -1);
    }
    
    private void handleClick() {
        if (!locationSelector.gameObject.activeSelf) return;
        locationSelector.localPosition = pointer.localPosition;
    }
}
}