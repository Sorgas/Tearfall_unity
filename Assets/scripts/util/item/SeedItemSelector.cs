using Leopotam.Ecs;

namespace util.item {
    public class SeedItemSelector : ItemSelector {
        private string plantName;
        public SeedItemSelector(string plant) {
            plantName = plant;
        }

        public override bool checkItem(EcsEntity item) {
            return false;
        }
    }
}