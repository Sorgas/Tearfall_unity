using System;
using System.Collections.Generic;
using enums.action;
using game.model.component.item;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

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
        public EcsEntity task;
        public ActionTarget target;
        public ActionStatusEnum status = ActionStatusEnum.OPEN;
        public LocalModel model => task.take<TaskActionsComponent>().model;

        public bool hasPerformer => task.IsAlive() && task.Has<TaskPerformerComponent>();

        public ref EcsEntity performer {
            get {
                ref var component = ref task.takeRef<TaskPerformerComponent>();
                return ref component.performer;
            }
        }
        // checked before starting performing, can create sub actions, can lock items
        public Func<ActionConditionStatusEnum> startCondition = () => ActionConditionStatusEnum.FAIL; // prevent starting empty action

        public System.Action onStart = () => { }; // performed on phase start
        public Action<EcsEntity, float> progressConsumer; // performs logic
        public Func<Boolean> finishCondition; // when reached, action ends
        public System.Action onFinish = () => { }; // performed on phase finish

        // should be set before performing
        public float progress = 0;
        public float speed = 1;
        public float maxProgress = 1;

        public Action(ActionTarget target) : this() {
            this.target = target;
        }

        public Action() {
            finishCondition = () => progress >= maxProgress;
            progressConsumer = (unit, delta) => progress += delta; // performs logic
        }

        // Performs action logic. Changes status.
        public void perform(EcsEntity unit) {
            if (status == ActionStatusEnum.OPEN) { // first execution of perform()
                status = ActionStatusEnum.ACTIVE;
                onStart.Invoke();
            }
            progressConsumer.Invoke(unit, speed);
            if (finishCondition.Invoke()) { // last execution of perform()
                onFinish.Invoke();
                status = ActionStatusEnum.COMPLETE;
            }
        }

        public ActionConditionStatusEnum addPreAction(Action action) {
            log("adding pre-action: " + action.name);
            task.take<TaskActionsComponent>().addFirstPreAction(action);
            action.task = task;
            return ActionConditionStatusEnum.NEW;
        }

        // TODO reference to task?
        protected void lockItems(List<EcsEntity> items) => items.ForEach(item => lockItem(item));

        // locks or unlocks item to task of this action. Item can be locked only to one task. 
        // Items are unlocked when task ends, see TaskCompletionSystem.
        protected void lockItem(EcsEntity item) {
            if (!itemCanBeLocked(item)) throw new ArgumentException("Cannot lock item. Item locked to another task");
            ref TaskLockedItemsComponent lockedItems = ref task.Get<TaskLockedItemsComponent>(); // can create component
            if (item.Has<LockedComponent>()) return; // item locked to this task
            item.Replace(new LockedComponent { task = task });
            lockedItems.lockedItems.Add(item);
            log("locking 1 item");
        }

        public bool itemCanBeLocked(EcsEntity item) {
            return !item.Has<LockedComponent>() || item.take<LockedComponent>().task == task;
        }

        // for visual progress bar. can be overriden in subclasses
        public virtual float getActionProgress() {
            if(maxProgress == 0) return 0;
            return progress / maxProgress;
        }

        protected void log(string message) => Debug.Log("[" + name + "]: " + message);
    }
}