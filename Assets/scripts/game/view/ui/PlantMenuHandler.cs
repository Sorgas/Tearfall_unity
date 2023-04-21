using game.input;
using game.model.component.plant;
using game.view.ui.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui {
    public class PlantMenuHandler : WindowManagerMenu {
        public const string name = "plant_menu";
        public Image image;
        public TextMeshProUGUI plantTitle;
        public TextMeshProUGUI plantGrowthText;
        public TextMeshProUGUI plantPropertiesText;

        public void fillForPlant(EcsEntity entity) {
            PlantComponent plant = entity.take<PlantComponent>();
            image.sprite = entity.take<PlantVisualComponent>().spriteRenderer.sprite;  
            plantTitle.text = plant.type.title;
            plantGrowthText.text = "Growth: " + 1 + " will grow in " + 1 + " days";
            string propertiesText = "";
            // TODO if(!tile lit) propertiesText += "Growth halted: no light \n"
            // TODO if(temperature low) propertiesText += "Growth slowed: low temperature: < 10 C"
            propertiesText += "Growth speed: 100%";
            plantPropertiesText.text = propertiesText;
        }

        public override string getName() => name;
    }
}