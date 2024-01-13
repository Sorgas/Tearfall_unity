using game.model;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.unit;
using UnityEngine;
using util.lang.extension;
using Transform = UnityEngine.Transform;

namespace game.view.system.unit {
// creates sprite GO for units without it. update GO position
public class UnitVisualSystem : IEcsRunSystem {
    public EcsFilter<UnitVisualComponent, UnitMovementComponent> filter;

    public void Run() {
        foreach (int i in filter) {
            ref EcsEntity unit = ref filter.GetEntity(i);
            ref UnitVisualComponent visual = ref filter.Get1(i);
            if (visual.handler == null) createSpriteGo(unit, ref visual);
            update(unit, ref visual);
        }
    }

    private void update(EcsEntity unit, ref UnitVisualComponent visual) {
        
        Transform transform = visual.handler.gameObject.transform;
        if (transform.localPosition == visual.target) return;
        
        UnitMovementComponent movement = unit.take<UnitMovementComponent>();
        float step = movement.step;
        transform.localPosition = Vector3.Lerp(visual.current, visual.target, step);
        // transform.localPosition
        // bool isOnRamp = GameModel.get().currentLocalModel.localMap.blockType.get(pos) == BlockTypes.RAMP.CODE
        //                 || unit.Has<UnitVisualOnBuildingComponent>();
        // // set mask to draw unit through z+1 toppings TODO use in movement
        // visual.handler.setMaskEnabled(isOnRamp);
        if (GlobalSettings.USE_SPRITE_SORTING_LAYERS) {
            Vector3Int pos = unit.pos();
            visual.handler.updateSpriteSorting(pos.z);
        }
        visual.handler.setOrientation(visual.orientation);
        
    }

    private void createSpriteGo(EcsEntity unit, ref UnitVisualComponent component) {
        CreatureType type = unit.take<UnitComponent>().type;
        Vector3Int pos = unit.pos();
        GameObject instance = PrefabLoader.create("Unit", GameView.get().sceneElementsReferences.mapHolder);
        instance.name = "Unit " + unit.name();
        
        component.current = ViewUtil.fromModelToSceneForUnit(pos, GameModel.get().currentLocalModel);
        component.target = ViewUtil.fromModelToSceneForUnit(pos, GameModel.get().currentLocalModel);
        UnitGoHandler handler = instance.GetComponent<UnitGoHandler>();
        component.handler = handler;
        component.orientation = SpriteOrientations.FR;
        handler.unit = unit;
        handler.headSprite.sprite = CreatureSpriteMap.get().getFor(type, "head", component.headVariant);
        handler.bodySprite.sprite = CreatureSpriteMap.get().getFor(type, "body", component.bodyVariant);
        handler.setOrientation(component.orientation);
    }
}
}