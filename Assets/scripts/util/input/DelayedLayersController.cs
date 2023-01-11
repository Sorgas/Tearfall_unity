using System;
using UnityEngine.InputSystem;

namespace util.input {
    public class DelayedLayersController : DelayedConditionController {

        public DelayedLayersController(InputAction inputAction, Action<int> callback) : base(null, null) {
            condition = () => inputAction.ReadValue<float>() != 0;
            action = () => callback.Invoke(inputAction.ReadValue<float>() > 0 ? 1 : -1);
        }
    }
}