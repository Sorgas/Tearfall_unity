using UnityEngine;

namespace Assets.scripts.util.input {
    // uses pressed key check as calling condition
    public class DelayedKeyController : DelayedConditionController{
        private KeyCode keycode;

        public DelayedKeyController(KeyCode keycode, System.Action action) : base(action, () => Input.GetKey(keycode)){
            this.keycode = keycode;
        }
    }
}