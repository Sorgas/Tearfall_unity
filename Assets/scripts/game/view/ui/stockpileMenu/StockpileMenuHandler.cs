using game.input;
using game.model.component;
using game.view.ui.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.geometry;
using util.lang.extension;

namespace game.view.ui.stockpileMenu {
    // menu for stockpiles. allows to do basic actions. allows to open config menu and presets menu.
    // TODO stats block, containers buttons
    public class StockpileMenuHandler : MbWindow, IHotKeyAcceptor {
        public const string name = "stockpile";
        public Button closeButton;
        public Button pauseButton;
        public Button priorityPlusButton;
        public Button priorityMinusButton;
        public Button presetsButton;
        public Button configButton;

        public StockpileConfigMenuHandler configMenuHandler;
        // public presetMenuHandler 

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI presetNameText;
        public Image presetIcon;

        private EcsEntity entity;
        private readonly IntRange priorityRange = new(1, 8);

        public void Start() {
            configButton.onClick.AddListener(() => toggleConfigMenu());
            closeButton.onClick.AddListener(close);
            pauseButton.onClick.AddListener(() => {
                ref StockpileComponent component = ref entity.takeRef<StockpileComponent>();
                component.paused = !component.paused;
            });
            priorityPlusButton.onClick.AddListener(() => changePriority(1));
            priorityMinusButton.onClick.AddListener(() => changePriority(-1));
            // presetsButton.onClick.AddListener(() => presetMenu.open());
        }

        public void initFor(EcsEntity entity) {
            this.entity = entity;
            nameText.text = entity.name();
            presetNameText.text = entity.take<StockpileComponent>().preset;
        }

        public bool accept(KeyCode key) {
            if (configMenuHandler.gameObject.activeSelf) return configMenuHandler.accept(key);
            if (key == KeyCode.Q) {
                WindowManager.get().closeWindow(name);
            }
            return false;
        }

        private void changePriority(int delta) {
            ref StockpileComponent component = ref entity.takeRef<StockpileComponent>();
            component.priority += delta;
        }

        private void toggleConfigMenu() {
            if (configMenuHandler.gameObject.activeSelf) {
                configMenuHandler.close();
            } else {
                configMenuHandler.openFor(entity);
            }
        }

        public override void close() {
            configMenuHandler.close();
            entity = EcsEntity.Null;
            base.close();
        }

        public override string getName() {
            return name;
        }
    }
}