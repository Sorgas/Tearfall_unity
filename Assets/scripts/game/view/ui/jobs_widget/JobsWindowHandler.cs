using enums.unit;
using game.model;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.jobs_widget {
    public class JobsWindowHandler : MbWindow, IHotKeyAcceptor {
        public RectTransform header;
        public RectTransform content;
        private const string iconPath = "prefabs/jobsmenu/JobIcon";
        private const string rowPath = "prefabs/jobsmenu/UnitJobRow";
        private const string togglePath = "prefabs/jobsmenu/JobToggle";

        private void Start() {
            fillIcons();
        }

        public override void open()  {
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

        // creates icons for actual JobsEnum
        private void fillIcons() {
            GameObject prefab = Resources.Load<GameObject>(iconPath);
            RectTransform textTransform = header.transform.GetComponentInChildren<Text>().gameObject.GetComponent<RectTransform>();
            float startX = textTransform.rect.width + textTransform.localPosition.x;
            float iconWidth = prefab.GetComponent<RectTransform>().rect.width;
            for (var i = 0; i < JobsEnum.jobs.Length; i++) {
                Job job = JobsEnum.jobs[i];
                GameObject icon = Instantiate(prefab, header.transform, false);
                icon.transform.localPosition = new Vector3(startX + iconWidth * i, 0, 0);
                Sprite sprite = Resources.Load<Sprite>("icons/jobs/" + job.iconName);
                icon.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            }
        }

        // creates rows for all units
        private void fillRows() {
            EcsFilter filter = GameModel.ecsWorld.GetFilter(typeof(EcsFilter<JobsComponent>));
            GameObject rowPrefab = Resources.Load<GameObject>(rowPath);
            float rowHeight = rowPrefab.GetComponent<RectTransform>().rect.height;
            foreach (var i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                JobsComponent? component = unit.get<JobsComponent>();
                if (component.HasValue) {
                    GameObject row = Instantiate(rowPrefab, content.transform, false);
                    row.transform.localPosition = new Vector3(0, -rowHeight * i, 0);
                    row.GetComponentInChildren<Text>().text = getUnitName(unit);
                    createToggles(row, component.Value);
                }
            }
        }

        private void createToggles(GameObject row, JobsComponent component) {
            GameObject togglePrefab = Resources.Load<GameObject>(togglePath);
            RectTransform unitName = row.transform.GetChild(0).GetComponent<RectTransform>();
            float startX = unitName.rect.width + unitName.localPosition.x;
            float iconWidth = togglePrefab.GetComponent<RectTransform>().rect.width;
            for (var i = 0; i < JobsEnum.jobs.Length; i++) {
                Job job = JobsEnum.jobs[i];
                GameObject toggle = Instantiate(togglePrefab, row.transform, false);
                toggle.transform.localPosition = new Vector3(startX + iconWidth * i, 0, 0);
                Toggle toggleComponent = toggle.GetComponentInChildren<Toggle>();
                toggleComponent.isOn = component.enabledJobs.Contains(job.name);
                toggleComponent.onValueChanged.AddListener(value => {
                    if (value) {
                        component.enabledJobs.Add(job.name);
                    } else {
                        component.enabledJobs.Remove(job.name);
                    }
                });
            }
        }

        private string getUnitName(EcsEntity unit) {
            NameComponent? name = unit.get<NameComponent>();
            return name.HasValue ? name.Value.name : "no name";
        }

        public override string getName() {
            return "jobs";
        }

        public bool accept(KeyCode key) {
            switch (key) {
                case KeyCode.J:
                case KeyCode.Q: 
                    WindowManager.get().closeWindow(this);
                    return true;
            }
            return false;
        }

        public void closeFromUI() {
            WindowManager.get().closeWindow(this);
        }
    }
}