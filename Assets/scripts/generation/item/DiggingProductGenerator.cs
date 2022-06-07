using enums.material;
using game.model;
using Leopotam.Ecs;
using types.material;

namespace generation.item {
    public class DiggingProductGenerator {
        public EcsEntity generate(int materialId) {
            Material_ material = MaterialMap.get().material(materialId);
            if (material.tags.Contains("stone")) return createItem("rock", material);
            if (material.tags.Contains("ore")) return createItem("ore", material);
            return EcsEntity.Null;
        }

        private EcsEntity createItem(string type, Material_ material) {
            EcsEntity item = GameModel.get().createEntity();
            return new ItemGenerator().generateItem(type, material.name, item);
        }
    }
}