using game.model.localmap;
using types;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace game.view.util {
// provides methods for converting scene screen and model positions to each other
public static class ViewUtil {
    private static Vector3 spriteOffset = new(0, 0.15f, UNIT_LAYER * GRID_STEP);
    private static Vector3 spriteOffsetOnRamp = new(0, 0.15f, UNIT_LAYER * GRID_STEP - GRID_STEP * 4f); // draw above walls

    // to scene position relative to mapHolder
    public static Vector3 fromModelToScene(Vector3Int pos) {
        return new Vector3(pos.x, pos.y + pos.z / 2f, -pos.z * 2f); // get scene position by model
    }

    public static Vector3 fromScreenToSceneGlobal(Vector3 pos, GameView view) {
        return view.sceneElements.mainCamera.ScreenToWorldPoint(pos);
    }

    public static Vector3 fromModelToSceneForUnit(Vector3Int position, LocalModel model) {
        bool isOnRamp = model.localMap.blockType.get(position) == BlockTypes.RAMP.CODE; // TODO or over building
        return fromModelToScene(position) + (isOnRamp ? spriteOffsetOnRamp : spriteOffset);
    }

    // z-distance between layers = 2
    // y-offset between layers = 0.5
    // camera height = -1 from current layer
    public static Vector3Int mouseScreenPositionToModel(Vector3 pos, GameView view) {
        Vector3 worldPosition = view.sceneElements.mainCamera.ScreenToWorldPoint(pos);
        Vector3 holderPos = view.sceneElements.mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        return new Vector3Int((int)holderPos.x, (int)(holderPos.y + holderPos.z / 4f + 0.25f), (int)(-holderPos.z / 2f));
    }
}
}