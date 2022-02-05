using Leopotam.Ecs;
using UnityEngine;

namespace test {
    // tests that in leoecs entities can be linked to each other,
    // and updating entity by a link from another entity is visible everywhere
    public class TestEcsSetup : MonoBehaviour{
        private EcsWorld world;
        private EcsSystems systems;
        private EcsEntity entity2;

        private void Start() {
            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems.Add(new ValuePrintSystem())
                .Add(new LinkIncrementSystem())
                .Init();

            EcsEntity entity1 = world.NewEntity();
            entity2 = world.NewEntity();
            entity1.Replace(new LinkComponent { link = entity2 });
            entity2.Replace(new ValueComponent { value = 0 });
        }
        
        private void Update() {
            systems.Run();
            entity2.Get<ValueComponent>().value += 1;
        }
    }

    class LinkIncrementSystem : IEcsRunSystem {
        public EcsFilter<LinkComponent> filter;
        
        public void Run() {
            foreach (var i in filter) {
                if (filter.GetEntity(i).Has<LinkComponent>()) {
                    LinkComponent linkComponent = filter.Get1(i);
                    if (linkComponent.link.Has<ValueComponent>()) {
                        ref var value = ref linkComponent.link.Get<ValueComponent>();
                        // value.value += 1;
                    } else {
                        Debug.Log("no value component !");
                    }
                }
            }
        }
    }
    
    class ValuePrintSystem : IEcsRunSystem {
        public EcsFilter<ValueComponent> filter;
        
        public void Run() {
            foreach (var i in filter) {
                Debug.Log(filter.Get1(i).value);
            }
        }
    }
    
    struct LinkComponent {
        public EcsEntity link;
    }

    struct ValueComponent {
        public int value;
    }
}