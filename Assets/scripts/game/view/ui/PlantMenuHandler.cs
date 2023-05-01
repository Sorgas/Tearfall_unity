using game.model.component.plant;
using game.view.ui.util;
using Leopotam.Ecs;
using TMPro;
using types.plant;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui {
    public class PlantMenuHandler : WindowManagerMenu {
        public const string name = "plant_menu";
        public Image image;
        public TextMeshProUGUI plantTitle;
        public TextMeshProUGUI plantGrowthText;
        public TextMeshProUGUI plantPropertiesText;
        private EcsEntity entity = EcsEntity.Null;

        public void fillForPlant(EcsEntity entity) {
            this.entity = entity;
            PlantComponent plant = entity.take<PlantComponent>();
            image.sprite = entity.take<PlantVisualComponent>().spriteRenderer.sprite;
            plantTitle.text = plant.type.title;
            updateText();
            string propertiesText = "";
            // TODO if(!tile lit) propertiesText += "Growth halted: no light \n"
            // TODO if(temperature low) propertiesText += "Growth slowed: low temperature: < 10 C"
            propertiesText += "Growth speed: 100%";
            plantPropertiesText.text = propertiesText;
        }

        private void Update() {
            if (entity == EcsEntity.Null) return;
            if (!entity.IsAlive()) return;
            updateText();
        }

        public override string getName() => name;

        private void updateText() {
            if (entity.Has<PlantGrowthComponent>()) {
                PlantGrowthComponent growth = entity.take<PlantGrowthComponent>();

                PlantType type = entity.take<PlantComponent>().type;
                float willGrow = type.maturityAge - growth.growth;
                plantGrowthText.text = $"Growth: {(growth.growth / growth.maturityAge):P0}, will grow in {willGrow:F1} days";
            } else {
                plantGrowthText.text = $"Fully grown";
            }
            if (entity.Has<PlantProductGrowthComponent>()) {
                PlantProductGrowthComponent growth = entity.take<PlantProductGrowthComponent>();
                plantGrowthText.text += $"\nProduct is growing: {(growth.growth / growth.growthEnd):P0},"
                                        + $" will grow in {growth.growthEnd - growth.growth:F1} days";
            } else if (entity.Has<PlantHarvestableComponent>()) {
                plantGrowthText.text += "\nReady for harvest.";
            }
        }
    }
}