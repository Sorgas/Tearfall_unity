using game.model.component.unit;
using game.view.ui.jobs_widget;
using Leopotam.Ecs;
using TMPro;
using types.unit;
using types.unit.skill;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
// Handler for skill/job row of a unit.
// shows priority toggling button if set for job
// shows level and exp bar if set for skill
// shows all if set for job with skill
public class SkillRowHandler : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI value;
    public Image bar;
    public Image barBackground;
    public Button priorityButton;
    public TextMeshProUGUI buttonText;
    public Button valueDebugButton;
    public bool debug = true;

    private EcsEntity unit;
    private string job;
    private UnitSkill skill;

    public void Start() {
        priorityButton.onClick.AddListener(() => changeJobPriority(true));
        priorityButton.gameObject.GetComponent<ButtonRightClickHandler>().onRmbClick.Add(() => {
            changeJobPriority(false);
        });
        valueDebugButton.onClick.AddListener(() => changeSkillValue(true));
        valueDebugButton.gameObject.GetComponent<ButtonRightClickHandler>().onRmbClick.Add(() => {
            changeSkillValue(false);
        });
    }

    public void updateValues() => showSkillElements();

    public void setForJob(EcsEntity unit, string job) {
        this.unit = unit;
        this.job = job;
        if (Jobs.jobMap[job].skill != null) { // job with skill
            string skillName = Jobs.jobMap[job].skill;
            skill = unit.take<UnitSkillComponent>().skills[skillName];
            title.text = capitalize(skill.type.name);
        } else { // jow without skill
            skill = null;
            title.text = capitalize(job);
        }
        showSkillElements();
        valueDebugButton.gameObject.SetActive(debug && skill != null);
        priorityButton.gameObject.SetActive(true);
        buttonText.text = unit.take<UnitJobsComponent>().enabledJobs[job].ToString();
    }

    public void setForSkill(EcsEntity unit, string skillName) {
        this.unit = unit;
        skill = unit.take<UnitSkillComponent>().skills[skillName];
        title.text = capitalize(skill.type.name);
        priorityButton.gameObject.SetActive(false);
        valueDebugButton.gameObject.SetActive(debug && skill != null);
        showSkillElements();
    }

    // shows skill level and exp bar
    private void showSkillElements() {
        value.text = skill != null ? skill.level.ToString() : "";
        if (skill == null || skill.level >= UnitSkills.MAX_VALUE) {
            barBackground.gameObject.SetActive(false);
        } else {
            barBackground.gameObject.SetActive(true);
            bar.fillAmount = 1f * skill.exp / UnitSkills.expValues[skill.level];
        }
    }

    private void changeJobPriority(bool increase) {
        unit.take<UnitJobsComponent>().changePriority(job, increase);
        buttonText.text = unit.take<UnitJobsComponent>().enabledJobs[job].ToString();
    }

    private void changeSkillValue(bool increase) {
        if (skill.level < UnitSkills.MAX_VALUE && increase) {
            skill.level += 1;
        }
        if (skill.level > UnitSkills.MIN_VALUE && !increase) {
            skill.level -= 1;
        }
        showSkillElements();
    }
    
    private string capitalize(string value) {
        return string.Concat(value[0].ToString().ToUpper(), value.Substring(1));
    }
}
}