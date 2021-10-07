using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {
    class TestSystem : IEcsRunSystem {
        EcsFilter<TestComponent> testFilter = null;

        public void Run() {
            foreach(int i in testFilter) {
                TestComponent qwer = testFilter.Get1(i);
                Debug.Log(qwer.value);
            }
        }
    }
}