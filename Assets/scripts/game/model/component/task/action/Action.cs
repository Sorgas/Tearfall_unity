using System;
using enums.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;
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
        public EcsEntity task;
        public ActionTarget target;
        public ActionStatusEnum status = ActionStatusEnum.OPEN;
        public ref EcsEntity performer {
            get {
                ref TaskComponents.TaskPerformerComponent component = ref task.takeRef<TaskComponents.TaskPerformerComponent>();
                return ref component.performer;
            }
        }

        /**
         * Condition to be met before task with this action is assigned to unit.
         * Should check tool, consumed items, target reachability for performer. 
         * Can use {@code task.performer}, as it is assigned to task before calling by {@link CreaturePlanningSystem}.
         */
        public Func<ActionConditionStatusEnum> startCondition = () => ActionConditionStatusEnum.FAIL; // prevent starting empty action
        public System.Action onStart = () => { }; // performed on phase start
        public Action<EcsEntity, float> progressConsumer; // performs logic
        public Func<Boolean> finishCondition; // when reached, action ends
        public System.Action onFinish = () => { }; // performed on phase finish

        public float progress;
        // should be set before performing
        public float speed = 1;
        public float maxProgress = 1;

        public Action(ActionTarget target) {
            this.target = target;
            finishCondition = () => progress >= maxProgress;
            progressConsumer = (unit, delta) => progress += delta; // performs logic
        }

        // Performs action logic. Changes status.
        public void perform(EcsEntity unit) {
            Debug.Log("performing action " + name);
            if (status == ActionStatusEnum.OPEN) { // first execution of perform()
                status = ActionStatusEnum.ACTIVE;
                onStart.Invoke();
            }
            progressConsumer.Invoke(unit, speed);
            if (finishCondition.Invoke()) { // last execution of perform()
                Debug.Log("action finished -- ");
                onFinish.Invoke();
                status = ActionStatusEnum.COMPLETE;
            }
        }

        public ActionConditionStatusEnum addPreAction(Action action) {
            Debug.Log("adding pre-action " + action.name);
            task.Get<TaskComponents.TaskActionsComponent>().addFirstPreAction(action);
            action.task = task;
            return ActionConditionStatusEnum.NEW;
        }

        protected void log(string message) {
            Debug.Log("[" + name + "]: " + message);
        }
    }
}