using System.Collections.Generic;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;

namespace generation.unit {
public class UnitSkillsGenerator {
    private WeightedSkills primarySkills = new();
    private WeightedSkills secondarySkills = new();

    public UnitSkillsGenerator() {
        addPrimarySkills();
        addSecondarySkills();
    }

    public void generate(EcsEntity unit) {
        unit.Replace(new UnitSkillComponent() { skills = new() });
        fillSkills(unit);
        addSkillLevels(unit);
    }

    private void fillSkills(EcsEntity unit) {
        Dictionary<string, UnitSkill> skills = unit.take<UnitSkillComponent>().skills;
        foreach (var skill in UnitSkills.allSkills) {
            addSkill(skills, skill, 0, 0);
        }
    }

    private void addSkillLevels(EcsEntity unit) {
        UnitSkillComponent component = unit.take<UnitSkillComponent>();
        string primary = primarySkills.getRandomSkill();
        int value = Random.Range(4, 9);
        component.skills[primary].value = value;
        
        for (int i = 0; i < 4; i++) {
            string skill = secondarySkills.getRandomSkill();
            int value2 = Random.Range(1, 3);
            component.skills[skill].value = value2;
        }
    }
    
    private void addSkill(Dictionary<string, UnitSkill> skills, string name, int value, int exp) {
        skills.Add(name, new UnitSkill(name, value, exp));
    }

    private void addPrimarySkills() {
        primarySkills.addSkill(UnitSkills.MINING, "miner", 10);
        primarySkills.addSkill(UnitSkills.WOODCUTTING, "woodcutter", 10);
        primarySkills.addSkill(UnitSkills.FARMING, "farmer", 10);
        primarySkills.addSkill(UnitSkills.FORAGING, "forager", 10);
        primarySkills.addSkill(UnitSkills.FISHING, "fisher", 10);
        primarySkills.addSkill(UnitSkills.CARPENTRY, "carpentry", 6);
        primarySkills.addSkill(UnitSkills.MASONRY, "masonry", 6);
        primarySkills.addSkill(UnitSkills.SMITHING, "blacksmith", 6);
        primarySkills.addSkill(UnitSkills.TAILORING, "tailor", 6);
        primarySkills.addSkill(UnitSkills.COOKING, "cook", 6);
        primarySkills.addSkill(UnitSkills.SCHOLARSHIP, "scholar", 1);
        primarySkills.addSkill(UnitSkills.MEDICINE, "doctor", 1);
        primarySkills.addSkill(UnitSkills.ALCHEMY, "alchemist", 1);
        primarySkills.addSkill(UnitSkills.TRADING, "trader", 1);
        primarySkills.addSkill(UnitSkills.PERFORMANCE, "performer", 1);
        primarySkills.addSkill(UnitSkills.MELEE, "fighter", 4);
        primarySkills.addSkill(UnitSkills.RANGED, "ranger", 4);
        primarySkills.addSkill(UnitSkills.MAGIC, "mage", 1);
    }

    private void addSecondarySkills() {
        secondarySkills.addSkill(UnitSkills.MINING, "", 3);
        secondarySkills.addSkill(UnitSkills.WOODCUTTING, "", 3);
        secondarySkills.addSkill(UnitSkills.FARMING, "", 3);
        secondarySkills.addSkill(UnitSkills.FORAGING, "", 3);
        secondarySkills.addSkill(UnitSkills.FISHING, "", 3);
        secondarySkills.addSkill(UnitSkills.CARPENTRY, "", 2);
        secondarySkills.addSkill(UnitSkills.MASONRY, "", 2);
        secondarySkills.addSkill(UnitSkills.SMITHING, "", 2);
        secondarySkills.addSkill(UnitSkills.TAILORING, "", 2);
        secondarySkills.addSkill(UnitSkills.COOKING, "", 2);
        secondarySkills.addSkill(UnitSkills.SCHOLARSHIP, "", 1);
        secondarySkills.addSkill(UnitSkills.MEDICINE, "", 2);
        secondarySkills.addSkill(UnitSkills.ALCHEMY, "", 1);
        secondarySkills.addSkill(UnitSkills.TRADING, "", 2);
        secondarySkills.addSkill(UnitSkills.PERFORMANCE, "", 2);
        secondarySkills.addSkill(UnitSkills.MELEE, "", 2);
        secondarySkills.addSkill(UnitSkills.RANGED, "", 2);
        secondarySkills.addSkill(UnitSkills.MAGIC, "", 1);
        secondarySkills.addSkill(UnitSkills.BLOCKING, "", 2);
        secondarySkills.addSkill(UnitSkills.DODGING, "", 2);
    }

    private class WeightedSkills {
        private List<WeightedSkill> skills = new();
        private int totalWeight = 0;

        public void addSkill(string skill, string professionName, int weight) {
            skills.Add(new WeightedSkill() { skill = skill, nickname = professionName, weight = totalWeight + weight });
            totalWeight += weight;
        }

        public string getRandomSkill() {
            float rand = Random.Range(0, 1f);
            float value = rand * totalWeight;
            foreach (var weightedSkill in skills) {
                if (value < weightedSkill.weight) {
                    return weightedSkill.skill;
                }
            }
            return "";
        }
    }
    
    private class WeightedSkill {
        public string skill;
        public string nickname;
        public int weight;
    }
}
}