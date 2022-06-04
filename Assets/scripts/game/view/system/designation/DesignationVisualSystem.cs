using enums.material;
using game.model;
using game.model.component;
using game.model.component.task;
using game.view.tilemaps;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.view.system.designation {
    // creates go with sprite for designations without visual component
    // TODO add system to udpate ramp orientation for construction
    public class DesignationVisualSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent>.Exclude<DesignationVisualComponent> filter;
        private RectTransform mapHolder;

        public DesignationVisualSystem() {
            mapHolder = GameView.get().sceneObjectsContainer.mapHolder;
        }

        public void Run() {
            foreach (var i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                DesignationComponent designation = filter.Get1(i);
                if (validateEntity(entity)) createSprite(entity, designation);
            }
        }

        private void createSprite(EcsEntity entity, DesignationComponent designation) {
            if (designation.type.SPRITE_NAME != null) {
                GameObject go = PrefabLoader.create("designation", mapHolder);
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                Sprite sprite = IconLoader.get("designation/" + designation.type.SPRITE_NAME);
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingOrder = entity.pos().z;
                float width = go.GetComponent<RectTransform>().rect.width;
                float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
                spriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
                go.transform.localPosition = getSpritePosition(entity.pos(), designation);
                entity.Replace(new DesignationVisualComponent { spriteRenderer = spriteRenderer });
            } else if (designation.type == DesignationTypes.D_CONSTRUCT) {
                DesignationConstructionComponent construction = entity.take<DesignationConstructionComponent>();
                Material_ material = MaterialMap.get().material(construction.material);
                string itemType = construction.itemType;
                GameObject go = PrefabLoader.create("designation", mapHolder);
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                BlockType blockType = BlockTypes.get(construction.type.blockTypeName);
                string spriteType = blockType == BlockTypes.RAMP ? "NE" : blockType.PREFIX;
                
                Sprite sprite = TileSetHolder.get().sprites[material + "_construction"][spriteType];
                
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingOrder = entity.pos().z;
                float width = go.GetComponent<RectTransform>().rect.width;
                float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
                spriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
                go.transform.localPosition = getSpritePosition(entity.pos(), designation);
                entity.Replace(new DesignationVisualComponent { spriteRenderer = spriteRenderer });
            }
        }

        private bool validateEntity(EcsEntity entity) {
            if (!entity.Has<PositionComponent>()) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.name + " has no PositionComponent");
                return false;
            }
            DesignationComponent designation = entity.Get<DesignationComponent>();
            if (designation.type != DesignationTypes.D_CONSTRUCT && designation.type.SPRITE_NAME == null) {
                Debug.LogWarning("designation " + entity.Get<DesignationComponent>().type.name + " has null spriteName");
                return false;
            }
            return true;
        }

        private Vector3 getSpritePosition(Vector3Int position, DesignationComponent designation) {
            Vector3 spritePosition = ViewUtil.fromModelToScene(position);
            spritePosition.z -= 0.1f;
            if (GameModel.localMap.blockType.getEnumValue(position) == BlockTypes.WALL) {
                spritePosition.y += 0.5f;
            }
            return spritePosition;
        }

        // private Sprite createSpriteForConstruction(DesignationComponent designation) {
        //     designation.
        // }
    }
}