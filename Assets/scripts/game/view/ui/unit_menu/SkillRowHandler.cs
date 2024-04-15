using game.model.component.unit;
using TMPro;
using types.unit.skill;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.unit_menu {
public class SkillRowHandler : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI value;
    public Image bar;
    public Button priorityButton;
    public TextMeshProUGUI buttonText;
    private UnitSkill skill;

    public void Start() {
        priorityButton.onClick.AddListener(() => {
            changeSkillPriority();
        });
    }
    
    public void set(UnitSkill skill) {
        this.skill = skill;
        title.text = skill.name;
        value.text = skill.value.ToString();
        buttonText.text = skill.priority.ToString();
        if (skill.value < UnitSkills.MAX_VALUE) {
            bar.gameObject.SetActive(true);
            bar.fillAmount = 1f * skill.exp / UnitSkills.expValues[skill.value];
        } else {
            bar.gameObject.SetActive(false);
        }
    }

    private void changeSkillPriority() {
        skill.priority++;
        if (skill.priority > 3) {
            skill.priority = 0;
        }
        buttonText.text = skill.priority.ToString();
    }
}
}