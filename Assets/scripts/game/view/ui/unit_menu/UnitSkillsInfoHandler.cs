using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using TMPro;
using types.unit;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class UnitSkillsInfoHandler : UnitMenuTab {
    public TextMeshProUGUI strengthAttributeText;
    public TextMeshProUGUI agilityAttributeText;
    public TextMeshProUGUI enduranceAttributeText;
    public TextMeshProUGUI intelligenceAttributeText;
    public TextMeshProUGUI willAttributeText;
    public TextMeshProUGUI charismaAttributeText;
    public RectTransform skillsPane;

    private Dictionary<string, SkillRowHandler> rows = new();

    public void Start() {
        findRows();
    }

    public override void showUnit(EcsEntity unit) {
        this.unit = unit;
        UnitPropertiesComponent component = unit.take<UnitPropertiesComponent>();
        strengthAttributeText.text = component.attributes[UnitAttributes.STRENGTH].value.ToString();
        agilityAttributeText.text = component.attributes[UnitAttributes.AGILITY].value.ToString();
        enduranceAttributeText.text = component.attributes[UnitAttributes.ENDURANCE].value.ToString();
        intelligenceAttributeText.text = component.attributes[UnitAttributes.INTELLIGENCE].value.ToString();
        willAttributeText.text = component.attributes[UnitAttributes.SPIRIT].value.ToString();
        charismaAttributeText.text = component.attributes[UnitAttributes.CHARISMA].value.ToString();
        setSkillValues(unit);
    }

    protected override void updateView() {
        foreach (SkillRowHandler row in rows.Values) {
            row.updateValues();
        }
    }

    private void setSkillValues(EcsEntity unit) {
        foreach (string rowName in rows.Keys) {
            if(Jobs.jobMap.Keys.Contains(rowName)) {
                rows[rowName].setForJob(unit, rowName);
            } else if (UnitSkills.skills.Keys.Contains(rowName)) {
                if (Jobs.jobsBySkill.ContainsKey(rowName)) {
                    Job job = Jobs.jobsBySkill[rowName];
                    rows[rowName].setForJob(unit, job.name);
                } else {
                    rows[rowName].setForSkill(unit, rowName);
                }
            } else {
                Debug.LogError($"Skill or job {rowName} not found.");
            }
        }
    }

    // searches skill row objects in children. For jobs, objects should be named as jobs. For skills without jobs objects should be named as skills.
    private void findRows() {
        foreach (Transform transform in skillsPane) {
            if (transform.gameObject.hasComponent<SkillRowHandler>()) {
                rows.Add(transform.gameObject.name.ToLower(), transform.gameObject.GetComponent<SkillRowHandler>());
            }
        }
    }
}
}