using enums;
using game.model;
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
            GameObject go = PrefabLoader.create("designation", GameView.get().mapHolder);
            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
            Sprite sprite = IconLoader.get("designation/" + designation.type.SPRITE_NAME);
            spriteRenderer.sprite = sprite;
            
            float width = go.GetComponent<RectTransform>().rect.width;
            float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
            spriteRenderer.transform.localScale = new Vector3(scale, scale,1);
            
            go.transform.localPosition = getSpritePosition(position, designation);
            // add visual component
            entity.Replace(new DesignationVisualComponent {spriteRenderer = spriteRenderer});
        }

        private bool validateEntity(EcsEntity entity) {
            if (!entity.Has<PositionComponent>()) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.NAME + " has no PositionComponent");
                return false;
            }
            DesignationComponent designation = entity.Get<DesignationComponent>();
            if (designation.type.SPRITE_NAME == null) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.NAME + " has null spriteName");
                return false;
            }
            return true;
        }

        private Vector3 getSpritePosition(PositionComponent position, DesignationComponent designation) {
            Vector3 spritePosition = ViewUtil.fromModelToScene(position.position);
            spritePosition.z -= 0.1f;
            if (GameModel.localMap.blockType.getEnumValue(position.position) == BlockTypeEnum.WALL) { // TODO elevate sprite on tall tiles
                spritePosition.y += 0.5f;
            }
            return spritePosition;
        }
    }
}