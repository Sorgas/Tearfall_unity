using enums;
using game.model;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEditor;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.unit {
    // creates sprite GO for units without it. update GO position
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitMovementComponent> filter;
        private RectTransform mapHolder;
        private Vector3 spriteOffset;
        private Vector3 spriteOffsetOnRamp;

        public UnitVisualSystem() {
            mapHolder = GameView.get().sceneObjectsContainer.mapHolder;
            spriteOffset = new(0, 0.25f, WALL_LAYER * GRID_STEP + GRID_STEP / 2);
            spriteOffsetOnRamp = spriteOffset + new Vector3(0, 0, -0.1f);
        }

        public void Run() {
            foreach (int i in filter) {
                ref EcsEntity entity = ref filter.GetEntity(i);
                ref UnitVisualComponent component = ref filter.Get1(i);
                UnitMovementComponent unitMovement = filter.Get2(i);
                if (component.handler == null) createSpriteGo(ref component);
                updatePosition(ref entity, ref component, unitMovement);
            }
        }

        private void updatePosition(ref EcsEntity unit, ref UnitVisualComponent component, UnitMovementComponent unitMovement) {
            // TODO use view utils
            Vector3Int pos = unit.pos();
            bool isOnRamp = GameModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE;
            component.handler.gameObject.transform.localPosition = 
                ViewUtil.fromModelToScene(pos) + (isOnRamp ? spriteOffsetOnRamp : spriteOffset);
            // set mask to draw unit through z+1 toppings
            component.handler.setMaskEnabled(isOnRamp);
            component.handler.updateZ(pos.z);
        }

        private void createSpriteGo(ref UnitVisualComponent component) {
            //TODO use prefabLoader
            GameObject prefab = Resources.Load<GameObject>("prefabs/Unit");
            GameObject instance = Object.Instantiate(prefab, new Vector3(), Quaternion.identity);
            component.handler = instance.GetComponent<UnitGoHandler>();
            instance.transform.SetParent(mapHolder);
        }
    }
}