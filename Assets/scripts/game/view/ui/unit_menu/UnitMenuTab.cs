using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.unit_menu {
// Tab of unit menu
public abstract class UnitMenuTab : MonoBehaviour, ICloseable {
    protected EcsEntity unit;

    public void open() {
        gameObject.SetActive(true);
    }

    public void close() {
        gameObject.SetActive(false);
    }

    // should display values which are not changed during update
    public virtual void showUnit(EcsEntity unit) {
        this.unit = unit;
    }

    public void Update() {
        if (unit != EcsEntity.Null) updateView();
    }

    // override to update display data on each frame
    protected virtual void updateView() { }
}
}