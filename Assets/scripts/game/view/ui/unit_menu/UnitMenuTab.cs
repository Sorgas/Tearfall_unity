using game.view.ui;
using Leopotam.Ecs;
using UnityEngine;

public abstract class UnitMenuTab : MonoBehaviour, ICloseable, IUnitMenuTab {

    public void open() {
        this.gameObject.SetActive(true);
    }

    public void close() {
        this.gameObject.SetActive(false);
    }

    public abstract void initFor(EcsEntity unit);
}