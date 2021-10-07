using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.unit {
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, MovementComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ref UnitVisualComponent component = ref filter.Get1(i);
                MovementComponent movement = filter.Get2(i);
                if (component.spriteRenderer == null) {
                    createUnit(ref component, movement);
                }
                updatePosition(ref component, movement);
            }
        }

        private void updatePosition(ref UnitVisualComponent component, MovementComponent movement) {
            Vector3Int position = movement.position;
            component.spriteRenderer.gameObject.transform.localPosition = new Vector3(position.x, position.y + position.z / 2f + 0.25f, - position.z * 2 - 0.1f);
        }

        private void createUnit(ref UnitVisualComponent component, MovementComponent movement) {
            GameObject prefab = Resources.Load<GameObject>("prefabs/Unit");
            GameObject instance = GameObject.Instantiate(prefab, new Vector3(), Quaternion.identity);
            component.spriteRenderer = instance.GetComponent<SpriteRenderer>();
            instance.transform.SetParent(GameView.get().mapHolder);
        }
    }
}