using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.unit_menu {
    public abstract class UnitMenuTab : MonoBehaviour, ICloseable, IUnitMenuTab {

        public void open() {
            this.gameObject.SetActive(true);
        }

        public void close() {
            this.gameObject.SetActive(false);
        }

        public abstract void initFor(EcsEntity unit);
    }
}