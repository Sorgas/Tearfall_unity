using System.Collections.Generic;

namespace enums.item.type {
    public class ToolItemType {
        public List<ToolAction> actions = new List<ToolAction>();  // some job, (mining, lumbering) require tools with specific name.
        public List<ToolAttack> attacks = new List<ToolAttack>();  // creatures will choose tools with best attack characteristics to use in combat.
        public string usedSkill; //TODO replace with enum

        public class ToolAction {
            public string action; // name name
            public float speedMod; // efficiency
            public string part;
        }

        public class ToolAttack {
            public string attack; //TODO replace with enum
            public float damageMod; // item efficiency or this attack
            public float baseReload; // attack reload turns
            public string damageType; //TODO replace with enum
            public string ammo; // ammo item name
            public string part;
        }
    }
}