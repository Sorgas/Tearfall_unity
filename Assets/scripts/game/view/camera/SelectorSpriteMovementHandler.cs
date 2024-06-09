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

        // private void updateText(Vector3Int modelPosition) {
        //     if (!map.inMap(modelPosition)) return;
        //     debugLabelText.text =
        //         $"pos: [{modelPosition.x},  {modelPosition.y},  {modelPosition.z}]\n" +
        //         $"block: {map.blockType.getEnumValue(modelPosition).NAME} {MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name}\n" +
        //         $"passage: {map.passageMap.getPassageType(modelPosition).name}\n" +
        //         $"area: {map.passageMap.defaultHelper.getArea(modelPosition)}\n" +
        //         $"area(rooms): {map.passageMap.groundNoDoorsHelper.getArea(modelPosition)}\n" + 
        //         $"area(fly): {map.passageMap.indoorHelper.getArea(modelPosition)}\n" + 
        //         $"UPS: {GameModel.get().counter.lastUps}"; 
        // }
    }
}