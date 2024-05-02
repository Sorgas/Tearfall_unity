using game.model.component.unit;
using game.view.ui.jobs_widget;
using Leopotam.Ecs;
using TMPro;
using types.unit;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace game.view.ui.jobs_window {
public class UnitJobButtonHandler : MonoBehaviour {
    public Button button;
    public Image image;
    public TextMeshProUGUI text;

    public void setFor(EcsEntity unit, string job) {
        UnitJobsComponent component = unit.take<UnitJobsComponent>();
        setButtonState(unit, job);
        button.onClick.AddListener(() => {
            component.changePriority(job, true);
            setButtonState(unit, job);
        });
        ButtonRightClickHandler rightClickHandler = gameObject.GetComponentInChildren<ButtonRightClickHandler>();
        rightClickHandler.onRmbClick.Add(() => {
            component.changePriority(job, false);
            setButtonState(unit, job);
        });
    }

    // sets priority to button text and skill level to color
    private void setButtonState(EcsEntity unit, string job) {
        UnitJobsComponent component = unit.take<UnitJobsComponent>();
        text.text = component.enabledJobs[job].ToString();
        Job jo = Jobs.jobMap[job];
        if (jo.skill != null) {
            int skillLevel = unit.take<UnitSkillComponent>().skills[jo.skill].level;
            image.color = UnitSkills.getColor(skillLevel, image.color);
        }
    }
}
}