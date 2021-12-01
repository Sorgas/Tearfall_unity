using System.Collections.Generic;
using UnityEngine;

namespace game.model.component.item {

    // main component for all items
    public struct ItemComponent {
        public float weight;
        public int material;
        public int volume; // used for containers

        public string materialString; // for faster naming
    }

    public struct WearComponent {
        public List<string> bodyparts; // covers listed bodyparts
        public int insulation;
    }

    public struct ConditionComponent {
        public List<string> acceptedDamages;
    }

    // item can spoil and rot over time
    public struct RotComponent {
        public int speed; 
        public int value; // 0-100
    }
    
    // item can rust over time
    public struct RustComponent {
        public int speed; 
        public int value; // 0-100
    }
    
    // item can be burned in fire over time
    public struct BurnComponent {
        public int speed; 
        public int value; // 0-100
    }
    
    // item can be broken over time
    public struct BrakeComponent {
        public int speed; // incoming damage is multiplied to this
        public int value; // 0-100
    }

    // item is cannot be put on the ground
    public struct DestroyOnGroundComponent {}

    // item can be split into two smaller items keeping total size.
    public struct SplittableComponent {}

    // two items of same type can be merged into one.
    public struct MergeableComponent {}

    public struct ToolComponent {
        public string operation;
    }

    // item gives temporary effect when consumed
    public struct PotionEffectComponent {
        public string effect; // todo
    }

    // item gives effect when worn
    public struct EnchantmentEffectComponent {
        public string effect; // todo
    }

    // item (or building) is container as bag or bottle and con contain items
    public struct ContainerComponent {
        public string type; // liquid, solid or grain
        public int capacity;
    }

    // item is forbidden to any use by player
    public struct ForbiddenComponent {}

    // item can be used as weapon in combat
    public struct CombatComponent {
        public string damageType;
        public int damage;
        public int hitReload;
        public int blockEfficiency;
    }

    // item is cursed and will put negative effect when used
    public struct CursedComponent {
        bool revealed; // curses are initially hidden
        string effect; // todo
    }

    // item is owned by some creature. creatures not hostile to owner won't use it
    public struct OwnedComponent {
        public string owner;
    }

    // item is held or worn by creature. it will not take part in non-hostile operations
    public struct OccupiedComponent {
        public string holder;
    }
    
    // item is held or worn by creature. it will not take part in non-hostile operations
    public struct ItemVisualComponent {
        public SpriteRenderer spriteRenderer;
    }
}