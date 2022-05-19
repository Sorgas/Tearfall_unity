using enums;
using game.model;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system.unit {
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitMovementComponent> filter;
        private RectTransform mapHolder;
        private float spriteZ = WALL_LAYER * GRID_STEP + GRID_STEP / 2;

        public UnitVisualSystem() {
            mapHolder = GameView.get().sceneObjectsContainer.mapHolder;
        }

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ref UnitVisualComponent component = ref filter.Get1(i);
                UnitMovementComponent unitMovement = filter.Get2(i);
                PositionComponent positionComponent = entity.Get<PositionComponent>();
                if (component.spriteRenderer == null) {
                    createUnit(ref component);
                }
                updatePosition(ref component, unitMovement, positionComponent);
            }
        }

        private void updatePosition(ref UnitVisualComponent component, UnitMovementComponent unitMovement, PositionComponent positionComponent) {
            // TODO use view utils
            Vector3Int pos = positionComponent.position;
            Vector3 spritePos = new Vector3(pos.x, pos.y + pos.z / 2f + 0.25f, -pos.z * 2 + spriteZ);
            if (GameModel.localMap.blockType.get(pos) == BlockTypeEnum.RAMP.CODE) {
                spritePos.z -= 0.1f;
            }
            component.spriteRenderer.gameObject.transform.localPosition = spritePos;
        }

        private void createUnit(ref UnitVisualComponent component) {
            //TODO use prefabLoader
            GameObject prefab = Resources.Load<GameObject>("prefabs/Unit");
            GameObject instance = Object.Instantiate(prefab, new Vector3(), Quaternion.identity);
            component.spriteRenderer = instance.GetComponent<SpriteRenderer>();
            instance.transform.SetParent(mapHolder);
        }
    }
}