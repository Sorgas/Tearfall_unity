using game.view.ui;
using Leopotam.Ecs;
using UnityEngine;

public class WorkbenchRecipeListHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
    private WorkbenchWindowHandler workbenchWindow;

    // fills recipes available in workbench
    public void fillFor(EcsEntity entity) {
        // TODO
        // TODO filter by knowledges
    }

    public void addOrder(string orderName) {

    }

    public void close() {
        throw new System.NotImplementedException();
    }

    public void open() {
        throw new System.NotImplementedException();
    }

    public bool accept(KeyCode key) {
        throw new System.NotImplementedException();
    }
}