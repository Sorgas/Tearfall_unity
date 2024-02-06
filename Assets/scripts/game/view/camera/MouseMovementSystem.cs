using System;
using game.model;
using game.model.localmap;
using game.view.util;
using TMPro;
using types.material;
using UnityEngine;

namespace game.view.camera {
    // moves selector sprite on screen, updates stat text
    // updates selector sprite basing on tile
    public class MouseMovementSystem {
        private readonly RectTransform selectorSprite;
        private Vector3 target = new(0, 0, -1); // target for sprite GO in scene coords
        private Vector3Int modelTarget;
        private Vector3Int cacheTarget; // to avoid excess GO moving
        private Vector3 speed; // keeps sprite speed between ticks
        // for debug only TODO decompose
        private readonly LocalMap map;
        private readonly TextMeshProUGUI debugLabelText;

        public MouseMovementSystem(LocalGameRunner initializer) {
            debugLabelText = initializer.sceneElementsReferences.modelDebugInfoPanel;
            selectorSprite = initializer.sceneElementsReferences.selector;
            map = GameModel.get().currentLocalModel.localMap;
        }

        public void update() {
            // move selector towards target
            updateText(modelTarget);
            if (selectorSprite.localPosition == target) return;
            selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, target, ref speed, 0.05f,
            float.PositiveInfinity, Time.unscaledDeltaTime); // move selector
            
            cacheTarget = modelTarget;
        }

        // update target when mouse moved (called from MIS), only way to update targets
        public void updateTarget(Vector3Int modelPosition) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            target.Set(scenePosition.x, scenePosition.y, scenePosition.z - 0.1f);
            modelTarget = modelPosition;
        }

        // update target and sprite position immediately (called from GV)
        public void updateTargetAndSprite(Vector3Int modelPosition) {
            updateTarget(modelPosition);
            selectorSprite.localPosition.Set(target.x, target.y, target.y);
        }

        private void updateText(Vector3Int modelPosition) {
            if (!map.inMap(modelPosition)) return;
            debugLabelText.text =
                $"pos: [{modelPosition.x},  {modelPosition.y},  {modelPosition.z}]\n" +
                $"block: {map.blockType.getEnumValue(modelPosition).NAME} {MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name}\n" +
                $"passage: {map.passageMap.getPassageType(modelPosition).name}\n" +
                $"area: {map.passageMap.defaultHelper.getArea(modelPosition)}\n" +
                $"area(rooms): {map.passageMap.groundNoDoorsHelper.getArea(modelPosition)}\n" + 
                $"area(fly): {map.passageMap.indoorHelper.getArea(modelPosition)}\n" + 
                $"UPS: {GameModel.get().counter.lastUps}"; 
        }
    }
}