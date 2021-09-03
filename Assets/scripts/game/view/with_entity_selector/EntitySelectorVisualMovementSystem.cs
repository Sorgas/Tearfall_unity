using Assets.scripts.game.model;
using Tearfall_unity.Assets.scripts.game.model.entity_selector;
using UnityEngine;

public class EntitySelectorVisualMovementSystem {
    // common
    public Camera camera;
    public RectTransform selectorSprite;
    public RectTransform mapHolder;
    // selector
    private EntitySelector selector = GameModel.get().selector;
    private EntitySelectorSystem system = GameModel.get().selectorSystem;
    private Vector3 selectorTarget = new Vector3(0, 0, -1); // target in scene coordinates
    private Vector3 selectorSpeed = new Vector3(); // keeps sprite speed between ticks

    Vector3Int cacheVector = new Vector3Int();

    public EntitySelectorVisualMovementSystem(Camera camera, RectTransform selectorSprite, RectTransform mapHolder) {
        this.camera = camera; 
        this.selectorSprite = selectorSprite;
        this.mapHolder = mapHolder;
    }

    public void update() {
        // Debug.Log("update "+ selector.position.x);
        
        selectorTarget.Set(selector.position.x, selector.position.y + selector.position.z / 2f, -2 * selector.position.z - 0.1f); // update target by in-model position
        if (selectorSprite.localPosition != selectorTarget)
            selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, selectorTarget, ref selectorSpeed, 0.05f); // move selector
    }

    // moves model position of selector and visual movement target. takes parameters in model coordinates
    // public Vector3Int moveSelector(int dx, int dy, int dz) {
    //     Vector3Int delta = system.moveSelector(dx, dy, dz); // update model position of selector
    //     return delta;
    // }

    // public void resetSelectorToMousePosition(Vector3 mousePosition) {
    //     Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);
    //     Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
    //     int zLayer = selector.position.z; // z-layer cannot be changed by moving mouse
    //     system.setSelectorPosition((int)mapHolderPosition.x, (int)(-zLayer / 2f + mapHolderPosition.y), zLayer);
    // }
}