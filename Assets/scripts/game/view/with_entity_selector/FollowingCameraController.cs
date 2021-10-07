using UnityEngine;
using util.geometry;

namespace game.view.with_entity_selector {
    public class LocalMapFollowingCameraController {
        private Camera camera;
        private RectTransform target;
        private Rect cameraRect;
        private Vector3 cacheVector = new Vector3();
        private FloatBounds2 visibleArea = new FloatBounds2();
        private Vector3 previousTargetPosition = new Vector3();

        public LocalMapFollowingCameraController(Camera camera, RectTransform target) {
            this.camera = camera;
            this.target = target;
        }
    }
}