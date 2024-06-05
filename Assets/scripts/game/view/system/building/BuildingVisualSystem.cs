using game.model.component.building;
using game.view.tilemaps;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.building {
    // creates sprite GO for building
    public class BuildingVisualSystem : IEcsRunSystem {
        public EcsFilter<BuildingComponent>.Exclude<BuildingVisualComponent> filter;

        private Vector3 spriteOffset;

        public BuildingVisualSystem() {
            spriteOffset = new(0, 0, WALL_LAYER * GRID_STEP);
        }

        public void Run() {
            foreach (int i in filter) {
                ref EcsEntity entity = ref filter.GetEntity(i);
                ref BuildingComponent component = ref filter.Get1(i);
                entity.Replace(new BuildingVisualComponent { gameObject = createSpriteGo(component, entity.pos()) });
            }
        }

        private GameObject createSpriteGo(BuildingComponent component, Vector3Int position) {
            GameObject instance = PrefabLoader.create("Building", GameView.get().sceneElements.mapHolder);
            SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
            Sprite sprite = BuildingTilesetHolder.get().get(component.type, component.orientation, 0);
            renderer.sprite = sprite;
            if (GlobalSettings.USE_SPRITE_SORTING_LAYERS) {
                renderer.sortingOrder = position.z;
            }
            float scale = getScale(sprite, component.type.size[OrientationUtil.isHorizontal(component.orientation) ? 1 : 0]);
            renderer.transform.localScale = new Vector3(scale, scale, 1);
            instance.transform.localPosition = ViewUtil.fromModelToScene(position) + spriteOffset;
            return instance;
        }

        private float getScale(Sprite sprite, int buildingWidth) {
            return buildingWidth * sprite.pixelsPerUnit / sprite.rect.width;
        }
    }
}