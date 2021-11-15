using UnityEngine;

namespace game.view.ui {
    public interface IHotKeyAcceptor {
        public bool accept(KeyCode key);
    }
}