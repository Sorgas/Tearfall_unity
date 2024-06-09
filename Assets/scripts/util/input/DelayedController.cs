namespace util.input {
// translates continuous input into sequence of action calls with specified delays.
// Each delay in array except the last one is used only once. 
// Passed delta time is accumulated. 
// When accumulated time value reaches value of current delay, time is reset, action is called, next delay is selected.
// accepts sequence of calls and performs action.
// First call starts counting of time. When time passes in amount of 'currentDelay' from 'delays', action is invoked, and next delay is taken.
// Last delay is used repeatedly. 
// Expects time between call() to be smaller than all delays
    public class DelayedController {
        protected System.Action action;
        protected float[] delays = { 0, 0.3f, 0.2f, 0.1f, 0.08f }; // first 0 means first call always invokes action
        private int currentDelay = 0;
        private float time = 0f; // time from last activation

        public DelayedController(System.Action action) {
            this.action = action;
        }

        // meant to be called continuously with random intervals
        public void call(float deltaTime) {
            float delay = delays[currentDelay];
            time += deltaTime; // accumulate time
            if (time > delay) { // enough time accumulated
                time -= delay; // keep extra time
                action.Invoke();
                if (currentDelay < delays.Length - 1) currentDelay++; // switch to next delay, if there is one
            }
        }

        public void reset() {
            time = 0;
            currentDelay = 0;
        }
    }
}