using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// Action for attacking single target. Performs attack, checks if target is down, puts weapon on cooldown.
// Selects first weapon in hands not on cooldown (for dual wield).
// Target is defined in children classes.
public abstract class MeleeAttackAction : Action {
    private int preparationTicks;
    protected bool lethal = true; // TODO

    public MeleeAttackAction(EntityActionTarget target) : base(target) {
        name = "attack melee action";
        displayProgressBar = false;
        startCondition = () => ActionCheckingEnum.OK;
        maxProgress = 1f;

        ticksConsumer = ticks => {
            if (!checkTargetValid()) return;
            GrabEquipmentSlot slot = performer.take<UnitEquipmentComponent>().getSlotWithoutCooldown();
            if (slot == null) return;
            EcsEntity weapon = slot.item;
            float roll = Random.Range(0, 1f);
            float chanceToHit = getChanceToHit(weapon);
            slot.cooldown = weapon != EcsEntity.Null ? weapon.take<ItemWeaponComponent>().reload : 30;
            startAnimation();
            if (roll < chanceToHit) {
                if (receiveAttack(roll, chanceToHit)) {
                    int damage = rollDamage(weapon);
                    string damageType = getDamageType(weapon);
                    takeDamage(damage, damageType);
                }
            }
        };
        finishCondition = () => {
            return !checkTargetValid(); // TODO check AttackInterruptedComponent
        };

        onFinish = () => { };
    }

    private float getChanceToHit(EcsEntity weaponEntity) {
        float accuracy = 0.5f;
        string skill = "melee";
        string attribute = "strength";
        if (weaponEntity != EcsEntity.Null) {
            ref ItemWeaponComponent weapon = ref weaponEntity.takeRef<ItemWeaponComponent>();
            accuracy = weapon.accuracy;
            skill = weapon.skill;
            attribute = weapon.attribute;
        }
        float skillFactor = 1 + performer.take<UnitSkillComponent>().skills[skill].level * 0.01f;
        float attackerStaminaFactor = performer.take<UnitHealthComponent>().stamina / 1f;
        float attackerAttributeFactor = 1 + (performer.take<UnitPropertiesComponent>().attributes[attribute].value - 10) / 2f;
        return accuracy * skillFactor * attackerStaminaFactor * attackerAttributeFactor;
    }

    private int rollDamage(EcsEntity weapon) {
        return weapon != EcsEntity.Null ? weapon.take<ItemWeaponComponent>().damage : 2;
    }

    // should return true if unit should continue attack on target
    protected abstract bool checkTargetValid();

    // should calculate target evasion and blocking and return true if hit lands
    protected abstract bool receiveAttack(float roll, float chanceToHit);

    protected abstract void takeDamage(int damage, string type);

    private void startAnimation() {
        UnitGoHandler handler = performer.take<UnitVisualComponent>().handler;
        handler.setOrientation(UnitOrientationsUtil.byVector(target.pos - performer.pos()));
        handler.actionAnimator.Play("attack"); // displays swing sprite animation
        handler.attackAnimator.Play("attackleft"); // moves unit sprite towards target
        handler.toggleProgressBar(false);
    }

    private string getDamageType(EcsEntity weapon) {
        return weapon != EcsEntity.Null ? weapon.take<ItemWeaponComponent>().damageType : "blunt";
    }
}
}