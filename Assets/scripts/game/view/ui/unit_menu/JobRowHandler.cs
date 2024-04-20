using game.model.component.unit;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class JobRowHandler : MonoBehaviour {
    public TextMeshProUGUI title;
    public Button priorityButton;
    public TextMeshProUGUI buttonText; // priority
    private EcsEntity unit;
    private string job;

    public void Start() {
        priorityButton.onClick.AddListener(() => {
            changeJobPriority();
        });
    }
    
    public void set(EcsEntity unit, string job) {
        this.unit = unit;
        this.job = job;
        title.text = job;
        buttonText.text = unit.take<UnitJobsComponent>().enabledJobs[job].ToString();
    }

    private void changeJobPriority() {
        ref UnitJobsComponent component = ref unit.takeRef<UnitJobsComponent>();
        component.enabledJobs[job]++;
        if (component.enabledJobs[job] > 3) {
            component.enabledJobs[job] = 0;
        }
        buttonText.text = component.enabledJobs[job].ToString(); 
    }
}
}