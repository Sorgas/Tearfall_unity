using UnityEngine;

namespace game.view.ui.util {
    public interface IHotKeyAcceptor {
        public bool accept(KeyCode key);
    }
}