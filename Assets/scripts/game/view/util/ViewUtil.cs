using UnityEngine;

namespace game.view.util {
    public class ViewUtil {
        public static Vector3 fromModelToScene(Vector3Int pos) {
            return new Vector3(pos.x, pos.y + pos.z / 2f, -pos.z * 2f - 0.1f); // get scene position by model
        }
    }
}