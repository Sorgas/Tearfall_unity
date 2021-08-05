using Leopotam.Ecs;
using UnityEngine;

namespace qwer {
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