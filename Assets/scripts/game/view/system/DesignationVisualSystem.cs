using game.model.component;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.DesignationComponents;

namespace game.view.system {

    // creates go with sprite for designations without visual component
    public class DesignationVisualSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent>.Exclude<DesignationVisualComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                if (validateEntity(entity)) createSprite(entity);
            }
        }

        private void createSprite(EcsEntity entity) {
            DesignationComponent designation = entity.Get<DesignationComponent>();
            PositionComponent position = entity.Get<PositionComponent>();
            // create sprite on scene
            GameObject go = PrefabLoader.create("designation");
            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = IconLoader.get("designation/" + designation.type.spriteName);
            Vector3 spritePosition = ViewUtil.fromModelToScene(position.position);
            spritePosition.z -= 0.1f;
            go.GetComponent<RectTransform>().localPosition = spritePosition;
            // add visual component
            entity.Replace(new DesignationVisualComponent {spriteRenderer = spriteRenderer});
        }

        private bool validateEntity(EcsEntity entity) {
            if (!entity.Has<PositionComponent>()) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.NAME + " has no PositionComponent");
                return false;
            }
            DesignationComponent designation = entity.Get<DesignationComponent>();
            if (designation.type.spriteName != null) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.NAME + " has null spriteName");
                return false;
            }
            return true;
        }
    }
}