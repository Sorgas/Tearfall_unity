using System.Collections.Generic;

namespace types.item.type {
    public class ToolItemType {
        public string action;  // some jobs, (mining, lumbering) require tools with specific name.
        public List<ToolAttack> attacks = new List<ToolAttack>();  // creatures will choose tools with best attack characteristics to use in combat.
        public string usedSkill; //TODO replace with enum

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