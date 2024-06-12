using game.view.util;
using UnityEngine;

namespace game.view.camera {
// Moves selector sprite on screen, updates stat text. Updates selector sprite basing on tile
// Only visual effects
// TODO move tile info to another monobeh
    public class SelectorSpriteMovementHandler : MonoBehaviour {
        public RectTransform selectorSprite;
        private Vector3 target = new(0, 0, -1); // target for sprite GO in scene coords
        private Vector3 speed; // keeps sprite speed between ticks
        
        // private readonly TextMeshProUGUI debugLabelText;

        // move selector towards target
        public void Update() {
            // updateText(modelTarget);
            if (GameView.get().cameraAndMouseHandler == null) return;
            updateTarget(GameView.get().cameraAndMouseHandler.selector.position);
            if (selectorSprite.localPosition == target) return;
            selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, target, ref speed, 0.05f,
            float.PositiveInfinity, Time.unscaledDeltaTime); // move selector
        }

        // update target when mouse moved (called from MIS), only way to update targets
        private void updateTarget(Vector3Int modelPosition) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f);
        }
    }
}