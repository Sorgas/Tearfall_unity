using UnityEngine;

namespace game.view {
    public interface IHotKeyAcceptor {
        public bool accept(KeyCode key);
    }
}