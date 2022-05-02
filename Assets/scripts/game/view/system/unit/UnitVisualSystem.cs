using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.unit {
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitMovementComponent> filter;
        private RectTransform mapHolder;
        
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
            Vector3Int position = positionComponent.position;
            component.spriteRenderer.gameObject.transform.localPosition = new Vector3(position.x, position.y + position.z / 2f + 0.25f, - position.z * 2 + 0.85f);
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