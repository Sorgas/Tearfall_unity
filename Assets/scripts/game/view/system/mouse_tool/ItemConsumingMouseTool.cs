using types.building;
using types.material;
using UnityEngine;

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
        
        protected bool fillSelectorForVariants(string buildingName, BuildingVariant[] variants) {
            hasMaterials = materialSelector.fill(buildingName, variants); // do not set tool if not enough materials
            materialSelector.selectFirst();
            materialSelector.open();
            if(!hasMaterials) Debug.LogWarning("materials for construction not found.");
            return hasMaterials;
        }
    }
}