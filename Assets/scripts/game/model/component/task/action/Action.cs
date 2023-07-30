using System;
using System.Collections.Generic;
using game.model.component.task.action.target;
using game.model.localmap;
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
        public EcsEntity task; // set when action is added to task
        public ActionTarget target;
        public ActionStatusEnum status = ActionStatusEnum.OPEN;

        protected LocalModel model => task.take<TaskActionsComponent>().model;
        protected ref EcsEntity performer => ref task.takeRef<TaskPerformerComponent>().performer;
        
        // checked before starting performing and before starting moving, can create sub actions, can lock items
        public Func<ActionConditionStatusEnum> startCondition = () => ActionConditionStatusEnum.FAIL; // prevent starting empty action

        protected System.Action onStart = () => { }; // performed on phase start
        protected Action<float> progressConsumer; // performs logic TODO consider removing all arguments except delta
        protected Func<Boolean> finishCondition; // when reached, action ends
        protected System.Action onFinish = () => { }; // performed on phase finish

        // should be set before performing. action is instant by default
        protected float speed = 1;
        public float progress = 0;
        public float maxProgress = 0;

        protected Action(ActionTarget target) : this() {
            this.target = target;
        }

        protected Action() {
            finishCondition = () => progress >= maxProgress;
            progressConsumer = delta => progress += delta; // performs logic
        }

        // Performs action logic. Changes status.
        public void perform(int ticks) {
            if (status == ActionStatusEnum.OPEN) { // first execution of perform()
                status = ActionStatusEnum.ACTIVE;
                onStart.Invoke();
            }
            progressConsumer.Invoke(speed * ticks);
            if (finishCondition.Invoke()) { // last execution of perform()
                onFinish.Invoke();
                status = ActionStatusEnum.COMPLETE;
            }
        }

        public ActionConditionStatusEnum addPreAction(Action action) {
            task.take<TaskActionsComponent>().addFirstPreAction(action);
            action.task = task;
            return ActionConditionStatusEnum.NEW;
        }
        
        protected void lockEntities(List<EcsEntity> entities) => ActionLockingUtility.lockEntities(entities, task);
        protected void unlockEntities(List<EcsEntity> entities) => ActionLockingUtility.unlockEntities(entities, task);
        protected void lockEntity(EcsEntity item) => ActionLockingUtility.lockEntity(item, task);
        protected void unlockEntity(EcsEntity item) => ActionLockingUtility.unlockEntity(item, task);
        protected bool entityCanBeLocked(EcsEntity item) => ActionLockingUtility.entityCanBeLocked(item, task);
        protected bool tileCanBeLocked(EcsEntity zone, Vector3Int tile) => ZoneUtils.tileCanBeLocked(zone, tile, task);
        protected void lockZoneTile(EcsEntity zone, Vector3Int tile) => ZoneUtils.lockZoneTile(zone, tile, task);
        protected void unlockZoneTile(EcsEntity zone, Vector3Int tile) => ZoneUtils.unlockZoneTile(zone, tile, task);
        
        protected void log(string message) => Debug.Log("[" + name + "]: " + message);
    }
}