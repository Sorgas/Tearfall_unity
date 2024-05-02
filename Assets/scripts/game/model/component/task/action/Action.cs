using System;
using System.Collections.Generic;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using game.model.system.unit;
using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
/**
* Action of a unit. All units behaviour except moving are defined in actions. Actions are parts of {@link Task}.
* During performing unit adds certain amount of 'work' to an action. Skills, health and other conditions may influence unit's work speed.
* <p>
* Action consist of several parts:
* <p>
* 1. Target where unit should be to perform action. Before taking action from container, target reachability is checked.
* 2. Taking condition - to be met before task is taken from container. 
* 3. Start condition - to be met before performing is started, can create additional actions e.g. bringing materials to workbench.
* 4. OnStart function - executed once.
* 5. Progress consumer function - executed many times. Does additive changes to model during action performing.
* 6. Finish condition - action finishes, when condition is met.
* 7. OnFinish function - executed once.
* <p>
* Additional actions are created and added to task, when start condition is not met, but could be after additional action(equip tool, bring items).
* If start condition is not met, action and its task are failed.
* Default implementation is an action with no requirements nor effect, which is finished immediately;
*/
    public abstract class Action {
        public string name = "base action";
        public string animation;
        public EcsEntity task; // set when action is added to task
        public ActionTarget target;
        public ActionStatusEnum status = ActionStatusEnum.OPEN;

        protected LocalModel model => task.take<TaskActionsComponent>().model;
        protected ref EcsEntity performer => ref task.takeRef<TaskPerformerComponent>().performer;
        
        // checked before starting performing and before starting moving, can create sub actions, can lock items
        public Func<EcsEntity, ActionCheckingEnum> assignmentCondition; // checks assignment for unit
        public Func<ActionCheckingEnum> startCondition;

        public System.Action onStart = () => { }; // performed on phase start
        public Action<int> ticksConsumer; // performs logic based on elapsed ticks
        public Func<Boolean> finishCondition; // when reached, action ends
        public System.Action onFinish = () => { }; // performed on phase finish

        // should be set before performing. action is instant by default
        protected float speed = 1;
        public float progress = 0;
        public float maxProgress = 0;
        protected string usedSkill;
        public bool debug = false;

        // target can be null
        protected Action(ActionTarget target) {
            this.target = target;
            assignmentCondition = (unit) => throw new NotImplementedException($"assignment condition for action {name} not implemented."); // prevent assigning unimplemented action
            startCondition = () => throw new NotImplementedException($"start condition for action {name} not implemented."); // prevent assigning unimplemented action
            finishCondition = () => progress >= maxProgress;
            ticksConsumer = ticks => progress += ticks * speed; // performs logic
        }
        
        public ActionCheckingEnum addPreAction(Action action) {
            task.take<TaskActionsComponent>().addFirstPreAction(action);
            action.task = task;
            return ActionCheckingEnum.NEW;
        }

        // should return false only if fail reason is unfixable by sub actions and action should be failed
        public virtual bool validate() => true;
        protected void lockEntities(List<EcsEntity> entities) => ActionLockingUtility.lockEntities(entities, task);
        protected void unlockEntities(List<EcsEntity> entities) => ActionLockingUtility.unlockEntities(entities, task);
        protected void lockEntity(EcsEntity item) => ActionLockingUtility.lockEntity(item, task);
        protected void unlockEntity(EcsEntity item) => ActionLockingUtility.unlockEntity(item, task);
        protected bool entityCanBeLocked(EcsEntity item) => ActionLockingUtility.entityCanBeLocked(item, task);
        protected bool tileCanBeLocked(EcsEntity zone, Vector3Int tile) => ZoneUtils.tileCanBeLocked(zone, tile, task);
        protected void lockZoneTile(EcsEntity zone, Vector3Int tile) => ZoneUtils.lockZoneTile(zone, tile, task);
        protected void unlockZoneTile(EcsEntity zone, Vector3Int tile) => ZoneUtils.unlockZoneTile(zone, tile, task);
        protected void log(string message) => Debug.Log($"[{name}]: {message}");
        protected void logDebug(string message) {
            if (debug) Debug.Log($"[{name}]: {message}");
        }
        
        // TODO use performer health condition(work slowly when tired). 
        protected float getSpeed() {
            float workSpeed = 1f;
            if(usedSkill != null) workSpeed += performer.take<UnitSkillComponent>().skills[usedSkill].getSpeedChange();
            return workSpeed;
        }
}
}