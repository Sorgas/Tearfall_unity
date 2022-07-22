using types.building;
using types.material;

namespace game.view.system.mouse_tool {
    public abstract class ItemConsumingMouseTool : MouseTool {
        protected string visualMaterial;
        protected bool hasMaterials; // TODO use to check before applying tool and for sprite tint
        protected string itemType;
        protected int material;

        public void setItem(string itemType, int material) {
            visualMaterial = MaterialMap.variateValue(MaterialMap.get().material(material).name, itemType);
            this.itemType = itemType;
            this.material = material;
            updateSprite();
        }
        
        protected bool fillSelectorForVariants(BuildingVariant[] variants) {
            hasMaterials = materialSelector.fill(variants); // do not set tool if not enough materials
            materialSelector.selectFirst();
            materialSelector.open();
            return hasMaterials;
        }
    }
}