using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using TMPro;
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
    
    private Dictionary<string, SkillRowHandler> skillRows = new ();

    public void Start() {
        findSkillRows();
    }
    
    public override void showUnit(EcsEntity unit) {
        UnitAttributesComponent attributes = unit.take<UnitAttributesComponent>();
        strengthAttributeText.text = attributes.strength.ToString();
        agilityAttributeText.text = attributes.agility.ToString();
        enduranceAttributeText.text = attributes.endurance.ToString();
        intelligenceAttributeText.text = attributes.intelligence.ToString();
        willAttributeText.text = attributes.will.ToString();
        charismaAttributeText.text = attributes.charisma.ToString();

        setSkillValues(unit);
    }
    
    protected override void updateView() {
        setSkillValues(unit);   
    }

    private void setSkillValues(EcsEntity unit) {
        UnitSkillComponent unitSkills = unit.take<UnitSkillComponent>();
        foreach (var skillName in UnitSkills.allSkills) {
            if (skillRows.ContainsKey(skillName)) {
                skillRows[skillName].set(unitSkills.skills[skillName]);
            }
        }
    }

    // searches skill row objects in children. objects should be named as skills
    private void findSkillRows() {
        foreach (Transform transform in skillsPane) {
            if (transform.gameObject.hasComponent<SkillRowHandler>()) {
                string name = transform.gameObject.name.ToLower();
                if(UnitSkills.allSkills.Contains(name)) {
                    skillRows.Add(name, transform.gameObject.GetComponent<SkillRowHandler>());
                } else {
                    Debug.LogError($"[UnitSkillInfoHandler]: unknown skill row with name {transform.gameObject.name}");
                }
            }
        }
    }
}
}