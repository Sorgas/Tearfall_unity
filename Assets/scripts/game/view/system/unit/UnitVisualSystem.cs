using game.model;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.unit {
    // creates sprite GO for units without it. update GO position
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitMovementComponent> filter;
        private Vector3 spriteOffset;
        private Vector3 spriteOffsetOnRamp;

        public UnitVisualSystem() {
            spriteOffset = new(0, 0.15f, WALL_LAYER * GRID_STEP);
            spriteOffsetOnRamp = new(0, 0.15f, WALL_LAYER * GRID_STEP - GRID_STEP / 2f); // draw above walls
        }

        public void Run() {
            foreach (int i in filter) {
                ref EcsEntity entity = ref filter.GetEntity(i);
                ref UnitVisualComponent component = ref filter.Get1(i);
                UnitMovementComponent unitMovement = filter.Get2(i);
                if (component.handler == null) createSpriteGo(entity, ref component);
                updatePosition(ref entity, ref component, unitMovement);
            }
        }

        private void updatePosition(ref EcsEntity unit, ref UnitVisualComponent component, UnitMovementComponent unitMovement) {
            // TODO use view utils
            Vector3Int pos = unit.pos();
            bool isOnRamp = GameModel.get().currentLocalModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE 
                    || unit.Has<UnitVisualOnBuildingComponent>();
            component.handler.gameObject.transform.localPosition = 
                ViewUtil.fromModelToScene(pos) + (isOnRamp ? spriteOffsetOnRamp : spriteOffset);
            // set mask to draw unit through z+1 toppings
            component.handler.setMaskEnabled(isOnRamp);
            component.handler.updateZ(pos.z);
        }

        private void createSpriteGo(EcsEntity unit, ref UnitVisualComponent component) {
            GameObject instance = PrefabLoader.create("Unit", GameView.get().sceneObjectsContainer.mapHolder);
            component.handler = instance.GetComponent<UnitGoHandler>();
            component.handler.unit = unit;
            // TODO set sprite to unit
        }
    }
}