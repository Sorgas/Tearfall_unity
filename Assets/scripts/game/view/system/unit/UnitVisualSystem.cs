using game.model;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.view.system.unit {
    // creates sprite GO for units without it. update GO position
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitMovementComponent> standingFilter;
        private float currentSpeed;

        public UnitVisualSystem() {
        }

        public void Run() {
            currentSpeed = GameModel.get().updateController.speed;
            foreach (int i in standingFilter) {
                ref EcsEntity unit = ref standingFilter.GetEntity(i);
                ref UnitVisualComponent visual = ref standingFilter.Get1(i);
                UnitMovementComponent unitMovement = standingFilter.Get2(i);
                if (visual.handler == null) createSpriteGo(unit, ref visual);
                updateForMovement(unit, ref visual);
            }
        }

        private void updateForMovement(EcsEntity unit, ref UnitVisualComponent visual) {
            Transform transform = visual.handler.gameObject.transform;
            if (transform.localPosition == visual.target) return;
            Vector3Int pos = unit.pos();
            float step = unit.take<UnitMovementComponent>().step;
            transform.localPosition = Vector3.Lerp(visual.current, visual.target, step);
            // transform.localPosition
            bool isOnRamp = GameModel.get().currentLocalModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE
                    || unit.Has<UnitVisualOnBuildingComponent>();
            // set mask to draw unit through z+1 toppings TODO use in movement
            visual.handler.setMaskEnabled(isOnRamp);
            visual.handler.updateZ(pos.z);
        }

        private void createSpriteGo(EcsEntity unit, ref UnitVisualComponent component) {
            Vector3Int pos = unit.pos();
            component.current = ViewUtil.fromModelToSceneForUnit(pos, GameModel.get().currentLocalModel);
            component.target = ViewUtil.fromModelToSceneForUnit(pos, GameModel.get().currentLocalModel);
            GameObject instance = PrefabLoader.create("Unit", GameView.get().sceneObjectsContainer.mapHolder);
            component.handler = instance.GetComponent<UnitGoHandler>();
            component.handler.unit = unit;
        }
    }
}