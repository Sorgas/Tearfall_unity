using game.model.component.building;
using game.view.tilemaps;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.view.system.building {
    public class BuildingVisualSystem : IEcsRunSystem {
        public EcsFilter<BuildingComponent>.Exclude<BuildingVisualComponent> filter;

        private Vector3 spriteOffset;

        public void Run() {
            foreach (int i in filter) {
                ref EcsEntity entity = ref filter.GetEntity(i);
                ref BuildingComponent component = ref filter.Get1(i);
                entity.Replace(new BuildingVisualComponent { gameObject = createSpriteGo(component, entity.pos()) });
            }
        }

        private GameObject createSpriteGo(BuildingComponent component, Vector3Int position) {
            GameObject instance = PrefabLoader.create("Building", GameView.get().sceneObjectsContainer.mapHolder);
            SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
            Sprite sprite = BuildingTilesetHolder.get().get(component.type, component.orientation);
            renderer.sprite = sprite;
            renderer.sortingOrder = position.z;
            float scale = getScale(sprite, component.type.size[OrientationUtil.isHorisontal(component.orientation) ? 1 : 0]);
            renderer.transform.localScale = new Vector3(scale, scale, 1);
            instance.transform.localPosition = ViewUtil.fromModelToScene(position);
            return instance;
        }

        private float getScale(Sprite sprite, int buildingWidth) {
            float spriteWidthInUnits = sprite.rect.width / sprite.pixelsPerUnit;
            return buildingWidth / spriteWidthInUnits;
        }
    }
}