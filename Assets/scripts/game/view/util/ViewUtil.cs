using UnityEngine;

namespace game.view.util {
    public class ViewUtil {
        public static Vector3 fromModelToScene(Vector3Int pos) {
            return new Vector3(pos.x, pos.y + pos.z / 2f, -pos.z * 2f - 0.1f); // get scene position by model
        }

        public static Vector3 fromSceneToModel(Vector3 pos) {
            return new Vector3((int)pos.x, (int)(pos.y + pos.z / 4f), -(pos.z + 1) / 2f);
        }
    }
}