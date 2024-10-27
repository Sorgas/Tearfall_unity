using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util;
using util.lang.extension;

namespace game.model.component.task.action.combat {
// MeleeAttackAction implementation which defines unit as a target
public class AttackUnitInMeleeAction : MeleeAttackAction {
    private EcsEntity targetUnit;

    public AttackUnitInMeleeAction(EcsEntity target) : base(new MeleeUnitActionTarget(target)) {
        name = "attack unit melee action";
        targetUnit = target;
    }

    protected override bool checkTargetValid() {
        if (lethal) {
        
            // TODO check if target has CorpseComponent
            return true;
        } else {
            // TODO check if downed
            throw new GameException("Non lethal combat not supported");
        }
    }

    protected override bool receiveAttack(float roll, float chanceToHit) {
        float targetStaminaFactor = 0.5f + targetUnit.take<UnitHealthComponent>().stamina / 2f;
        float skillFactor = 1 + targetUnit.take<UnitSkillComponent>().skills["melee"].level * 0.01f;
        EcsEntity shield = targetUnit.take<UnitEquipmentComponent>().getEquippedShieldWithoutCooldown();
        if (shield != EcsEntity.Null) {
            ref ItemShieldComponent component = ref shield.takeRef<ItemShieldComponent>();
            chanceToHit -= component.blockChance * skillFactor * targetStaminaFactor;
            if (roll > chanceToHit) {
                Debug.Log("blocked");
                // TODO decrease target stamina
                component.cooldown = component.reload;
                return false;
            }
        }
        // TODO parry here
        return true;
    }

    protected override void takeDamage(int damage, string type) {
        Debug.Log($"Taken {damage} {type} damage");
    }
}
}