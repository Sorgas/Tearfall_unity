using game.model.localmap;
using types;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace game.view.util {
    // provides methods for converting scene screen and model positions to each other
    public static class ViewUtil {
        private static Vector3 spriteOffset = new(0, 0.15f, UNIT_LAYER * GRID_STEP);
        private static Vector3 spriteOffsetOnRamp = new(0, 0.15f, UNIT_LAYER * GRID_STEP - GRID_STEP * 4f); // draw above walls

        public static Vector3 fromModelToScene(Vector3Int pos) {
            return new Vector3(pos.x, pos.y + pos.z / 2f, -pos.z * 2f); // get scene position by model
        }

        public static Vector3 fromSceneToModel(Vector3 pos) {
            return new Vector3((int)pos.x, (int)(pos.y + pos.z / 4f), -pos.z / 2f);
        }

        public static Vector3Int fromSceneToModelInt(Vector3 pos) {
            return new Vector3Int((int)pos.x, (int)(pos.y + pos.z / 4f), (int)(-pos.z / 2f));
        }

        public static Vector3Int fromScreenToModel(Vector3 pos, GameView view) {
            return fromSceneToModelInt(fromScreenToScene(pos, view));
        }

        public static Vector3 fromScreenToScene(Vector3 pos, GameView view) {
            Vector3 worldPosition = view.sceneObjectsContainer.mainCamera.ScreenToWorldPoint(pos);
            return view.sceneObjectsContainer.mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        }

        public static Vector3 fromScreenToSceneGlobal(Vector3 pos, GameView view) {
            return view.sceneObjectsContainer.mainCamera.ScreenToWorldPoint(pos);
        }

        public static Vector3 fromModelToSceneForUnit(Vector3Int position, LocalModel model) {
            bool isOnRamp = model.localMap.blockType.get(position) == BlockTypes.RAMP.CODE; // TODO or over building
            return fromModelToScene(position) + (isOnRamp ? spriteOffsetOnRamp : spriteOffset);
        }
    }
}