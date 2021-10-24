using enums.unit;
using game.model;
using game.model.component;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.jobs_widget {
    public class JobsWidgetHandler : MonoBehaviour {
        public RectTransform header;
        public RectTransform content;
        private string iconPath = "prefabs/jobsmenu/JobIcon";
        private string rowPath = "prefabs/jobsmenu/UnitJobRow";
        private string togglePath = "prefabs/jobsmenu/JobToggle";
        
        private void Start() {
            fillIcons();
            updateRowPrefab();
            fillRows();
        }

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

        private void updateRowPrefab() {
            GameObject togglePrefab = Resources.Load<GameObject>(togglePath);
            GameObject row2 = Resources.Load<GameObject>(rowPath);
            string path = AssetDatabase.GetAssetPath(row2);
            Debug.Log(path);
            GameObject rowPrefab = PrefabUtility.LoadPrefabContents(path);
            RectTransform unitName = rowPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float startX = unitName.rect.width + unitName.localPosition.x;
            float iconWidth = togglePrefab.GetComponent<RectTransform>().rect.width;
            for (var i = 0; i < JobsEnum.jobs.Length; i++) {
                GameObject toggle = Instantiate(togglePrefab, rowPrefab.transform, false);
                toggle.transform.localPosition = new Vector3(startX + iconWidth * i, 0, 0);
            }
            PrefabUtility.SaveAsPrefabAsset(rowPrefab, path);
            PrefabUtility.UnloadPrefabContents(rowPrefab);
        }
        
        private void fillRows() {
            GameObject rowPrefab = Resources.Load<GameObject>(rowPath);
            float rowHeight = rowPrefab.GetComponent<RectTransform>().rect.height;
            // add toggles to row prefab
            // add rows to scrollview
            EcsFilter filter = GameModel.ecsWorld.GetFilter(typeof(EcsFilter<JobsComponent>));
            foreach (var i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                JobsComponent? component = unit.get<JobsComponent>();
                NameComponent? name = unit.get<NameComponent>();
                if (component.HasValue) {
                    GameObject row = Instantiate(rowPrefab, content.transform, false);
                    row.GetComponentInChildren<Text>().text = name.HasValue ? name.Value.name : "no name";
                    row.transform.localPosition = new Vector3(0, -rowHeight * i, 0);
                }
            }
        }
    }
}