
// calls and resets itself using specified condition
using System;

public class DelayedConditionController : DelayedController {
    private Func<bool> condition;
    private bool active;

    public DelayedConditionController(System.Action action, Func<bool> condition) : base(action) {
        this.condition = condition;
    }

    public void update(float deltaTime) {
        if (condition.Invoke()) {
            base.call(deltaTime);
            active = true;
        } else if(active) {
            reset();
        }
    }

    public new void reset() {
        active = false;
        base.reset();
    }
}