using Leopotam.Ecs;
using types.material;

namespace generation.item {
    public class DiggingProductGenerator {
        public EcsEntity generate(int materialId, LocalModel model) {
            Material_ material = MaterialMap.get().material(materialId);
            if (material.tags.Contains("stone")) return createItem("rock", material, model);
            if (material.tags.Contains("ore")) return createItem("ore", material, model);
            return EcsEntity.Null;
        }

        private EcsEntity createItem(string type, Material_ material, LocalModel model) {
            return new ItemGenerator().generateItem(type, material.name, model.createEntity());
        }
    }
}