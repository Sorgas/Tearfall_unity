using game.input;
using game.model;
using game.model.component;
using game.model.component.unit;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.unit;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.jobs_window {
// Handles window for changing jobs for all settlers. Displays table of settlers and jobs.    
public class JobsWindowHandler : GameWindow {
        public const string name = "jobs";

        public RectTransform header;
        public RectTransform content;

        private int colWidth = 50;
        private int rowHeight = 30;
        private int titleWidth = 200;
        private void Start() {
            fillIcons();
        }

        public override void open() {
            base.open();
            deleteRows();
            fillRows();
        }

        // creates toggles associated to actual JobEnum, adds listeners
        private void deleteRows() {
            foreach (Transform child in content.transform) {
                Destroy(child.gameObject);
            }
        }

        // creates jobs icons in window header
        private void fillIcons() {
            for (var i = 0; i < Jobs.jobs.Length; i++) {
                GameObject icon = PrefabLoader.create("JobIcon", header.transform, new Vector3(titleWidth + colWidth * i, 0, 0));
                Sprite sprite = Resources.Load<Sprite>("icons/jobs/" + Jobs.jobs[i].iconName);
                icon.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            }
        }

        // creates rows for all units
        private void fillRows() {
            EcsFilter filter = GameModel.get().currentLocalModel.ecsWorld.GetFilter(typeof(EcsFilter<UnitJobsComponent>));
            foreach (var i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                GameObject row = PrefabLoader.create("UnitJobRow", content.transform, new Vector3(0, -rowHeight * i, 0));
                row.GetComponentInChildren<TextMeshProUGUI>().text = getUnitName(unit);
                createButtons(row, unit);
            }
        }

        // creates buttons for all jobs
        private void createButtons(GameObject row, EcsEntity unit) {
            for (var i = 0; i < Jobs.jobs.Length; i++) {
                Job job = Jobs.jobs[i];
                GameObject buttonGo = PrefabLoader.create("JobPriorityButton", row.transform, new Vector3(titleWidth + colWidth * i, 0, 0));
                UnitJobButtonHandler handler = buttonGo.GetComponent<UnitJobButtonHandler>();
                handler.setFor(unit, job.name);
            }
        }

        private string getUnitName(EcsEntity unit) {
            NameComponent? name = unit.optional<NameComponent>();
            return name.HasValue ? name.Value.name : "no name";
        }

        public override string getName() => name;

        public override bool accept(KeyCode key) {
            if (!base.accept(key)) {
                if (key == KeyCode.J) {
                    WindowManager.get().closeWindow(name);
                    return true;
                }
            }
            return false;
        }

        public void closeFromUI() {
            WindowManager.get().closeWindow(name);
        }
    }
}