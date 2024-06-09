using System;

namespace util.input {
// calls and resets itself using specified condition
// Adds condition to DelayedController. When updated, checks condition and only then calls DelayedController to run time with delays.
// If condition is failed, time counter is reset.
    public class DelayedConditionController : DelayedController {
        protected Func<bool> condition;
        private bool active;

        public DelayedConditionController(Action action, Func<bool> condition) : base(action) {
            this.condition = condition;
        }

        public void update(float deltaTime) {
            if (condition.Invoke()) {
                call(deltaTime);
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
}