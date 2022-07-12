using types.building;
using types.material;

namespace game.view.system.mouse_tool {
    public abstract class ItemConsumingMouseTool : MouseTool {
        protected string visualMaterial;

        public void setItem(string itemType, int material) {
            setItem(itemType, material);
            visualMaterial = MaterialMap.variateValue(MaterialMap.get().material(material).name, itemType);
            updateSprite(true);
        }
        
        protected bool fillSelectorForVariants(BuildingVariant[] variants) {
            bool hasMaterials = materialSelector.fill(variants); // do not set tool if not enough materials
            materialSelector.selectFirst();
            materialSelector.open();
            return hasMaterials;
        }
    }
}