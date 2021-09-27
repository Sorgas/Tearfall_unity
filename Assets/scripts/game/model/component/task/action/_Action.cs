using System;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.enums.action;
using UnityEngine;
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
public abstract class _Action {
    public string name;
    public TaskComponent task; // can be modified during execution
    public ActionTarget target;
    public ActionStatusEnum status = ActionStatusEnum.OPEN;
    protected string skill;

    /**
     * Condition to be met before task with this action is assigned to unit.
     * Should check tool, consumed items, target reachability for performer. 
     * Can use {@code task.performer}, as it is assigned to task before calling by {@link CreaturePlanningSystem}.
     */
    public Func<EcsEntity, ActionConditionStatusEnum> startCondition; // called before performing, can create sub actions
    public System.Action onStart; // performed on phase start
    public System.Action<EcsEntity, float> progressConsumer; // performs logic
    public Func<Boolean> finishCondition; // when reached, action ends
    public System.Action onFinish; // performed on phase finish

    public float progress;
    
    // should be set before performing
    public float speed = 1;
    public float maxProgress = 1;

    protected _Action(ActionTarget target) : this(target, null) {}

    public _Action(ActionTarget target, string skill) {
        this.target = target;
        if(skill != null && SkillMap.getSkill(skill) == null) Debug.LogError("Skill " + skill + " not found.");
        this.skill = skill;
        startCondition = (unit) => ActionConditionStatusEnum.FAIL; // prevent starting
        onStart = () => {};
        progressConsumer = (unit, delta) => progress += delta;
        finishCondition = () => progress >= maxProgress;
        onFinish = () => {};
        reset();
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

    public void reset() {
        speed = 1;
        progress = 0;
        maxProgress = 1;
    }

    public ActionConditionStatusEnum addPreAction(_Action action) {
        task.addFirstPreAction(action);
        return ActionConditionStatusEnum.NEW;
    }

    // protected float performance() {
    //     return task.performer.get(HealthAspect.class).stats.get(GameplayStatEnum.WORK_SPEED) + 
    //             0.05f * performerLevel();
    // }

    // protected int performerLevel() {
    //     return task.performer.get(JobSkillAspect.class).skills.get(skill).level();
    // }

    // protected Skill skill() {
    //     return SkillMap.getSkill(skill);
    // }
    public override string ToString() {
        return name;
    }
}
