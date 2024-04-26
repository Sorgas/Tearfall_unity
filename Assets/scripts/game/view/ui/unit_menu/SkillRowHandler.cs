using System.Collections.Generic;
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

    private EcsEntity unit;
    private string job;
    private UnitSkill skill;

    public void Start() {
        priorityButton.onClick.AddListener(() => changeJobPriority(true));
        priorityButton.gameObject.GetComponent<ButtonRightClickHandler>().onRmbClick.Add(() => {
            changeJobPriority(false);
        });
    }

    public void setForJob(EcsEntity unit, string job) {
        this.unit = unit;
        this.job = job;
        if (Jobs.jobMap[job].skill != null) { // job with skill
            string skillName = Jobs.jobMap[job].skill;
            skill = unit.take<UnitSkillComponent>().skills[skillName];
            title.text = capitalize(skill.name);
            showSkillElements(skill);
        } else { // jow without skill
            title.text = capitalize(job);
            showSkillElements(null);
        }
        priorityButton.gameObject.SetActive(true);
        buttonText.text = unit.take<UnitJobsComponent>().enabledJobs[job].ToString();
    }

    public void setForSkill(EcsEntity unit, string skillName) {
        this.unit = unit;
        skill = unit.take<UnitSkillComponent>().skills[skillName];
        title.text = capitalize(skill.name);
        priorityButton.gameObject.SetActive(false);
        showSkillElements(skill);
    }

    // shows skill level and exp bar
    private void showSkillElements(UnitSkill skill) {
        value.text = skill != null ? skill.value.ToString() : "";
        if (skill == null || skill.value >= UnitSkills.MAX_VALUE) {
            barBackground.gameObject.SetActive(false);
        } else {
            barBackground.gameObject.SetActive(true);
            bar.fillAmount = 1f * skill.exp / UnitSkills.expValues[skill.value];
        }
    }

    private void changeJobPriority(bool increase) {
        unit.take<UnitJobsComponent>().changePriority(job, increase);
        buttonText.text = unit.take<UnitJobsComponent>().enabledJobs[job].ToString();
    }

    private string capitalize(string value) {
        return string.Concat(value[0].ToString().ToUpper(), value.Substring(1));
    }
}
}