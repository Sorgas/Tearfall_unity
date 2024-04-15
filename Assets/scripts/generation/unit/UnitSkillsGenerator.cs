using System.Collections.Generic;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.skill;
using util.lang.extension;

namespace generation.unit {
public class UnitSkillsGenerator {

    public void generate(EcsEntity unit) {
        unit.Replace(new UnitSkillComponent() { skills = new() });
        fillSkills(unit);
    }
    
    private void fillSkills(EcsEntity unit) {
        Dictionary<string, UnitSkill> skills = unit.take<UnitSkillComponent>().skills;
        foreach (var skill in UnitSkills.allSkills) {
            addSkill(skills, skill, 0, 0);
        }
    }
    
    private void addSkill(Dictionary<string, UnitSkill> skills, string name, int value, int exp) {
        skills.Add(name, new UnitSkill(name, value, exp));
    }
}
}