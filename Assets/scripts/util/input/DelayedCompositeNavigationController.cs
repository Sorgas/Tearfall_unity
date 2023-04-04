using UnityEngine;
using UnityEngine.InputSystem;

namespace util.input {
    public class DelayedCompositeNavigationController : DelayedConditionController {
        private InputAction inputAction;

        public DelayedCompositeNavigationController(InputAction inputAction, System.Action<int,int> callback) : base(null, null) {
            delays = new float[] { 0 };
            this.inputAction = inputAction;
            action = () => {
                Vector2 vector = inputAction.ReadValue<Vector2>();
                callback.Invoke((int)vector.x, (int)vector.y);
            };
            condition = () => inputAction.ReadValue<Vector2>() != Vector2.zero;
        }
    }
}