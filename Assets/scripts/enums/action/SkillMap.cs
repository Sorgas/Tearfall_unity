using Assets.scripts.util.lang;

namespace Tearfall_unity.Assets.scripts.enums.action {
    public class SkillMap : Singleton<SkillMap>{

        public SkillMap() {}

        public static string getSkill(string skill) {
            return "none";
        }
    }
}