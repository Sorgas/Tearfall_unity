using Assets.scripts.game.model;
using Leopotam.Ecs;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view.system.unit {
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
            }
        }

        private void createUnit(ref UnitVisualComponent component, MovementComponent movement) {
            GameObject prefab = Resources.Load<GameObject>("prefabs/Unit");
            Vector3Int position = movement.position;
            GameObject instance = GameObject.Instantiate(prefab, new Vector3(), Quaternion.identity);
            Debug.Log("unit created " + position);
            component.spriteRenderer = instance.GetComponent<SpriteRenderer>();
            instance.transform.parent = GameView.get().mapHolder;
            instance.transform.localPosition = new Vector3(position.x, position.y + position.z / 2 + 0.25f, - position.z * 2 - 0.1f);
        }
    }
}