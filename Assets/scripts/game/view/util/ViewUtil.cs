using UnityEngine;

namespace game.view.util {
    public class ViewUtil {
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
    }
}