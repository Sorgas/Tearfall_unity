﻿using game.model.component.task.action.equipment.obtain;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.equipment.use {
    /**
    * Abstract action for putting items to different places like ground, containers, or equipment.
    * Only defines that item should be in performer's buffer.
    *
    * @author Alexander on 12.04.2020
    */
    public abstract class PutItemToDestinationAction : ItemAction {

        protected PutItemToDestinationAction(ActionTarget target, EcsEntity item) : base(target, item) {
            name = $"put {item.name()} to destination";
            startCondition = () => {
                if (!validate()) return FAIL;
                lockEntity(item);
                if (equipment.hauledItem != item) return addPreAction(new ObtainItemAction(item));
                return OK; // performer has item
            };

            onStart = () => maxProgress = 20;
        }

        // checks if item is hauled by unit and creates sub-action. to be called from startCondition
        protected ActionCheckingEnum verifyItemHauled() {
            if (equipment.hauledItem != item) return addPreAction(new ObtainItemAction(item));
            return OK;
        }
    }
}