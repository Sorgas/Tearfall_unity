using game.model.component.building;
using game.view.ui;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

public class WorkbenchWindowHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
    public TextMeshProUGUI workbenchNameText;
    public Button addOrderButton;
    public GameObject recipesPopup;
    public GameObject orderList;

    // fill wb orders
    public void init(EcsEntity entity) {
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        workbenchNameText.text = workbench.name;

    }

    // 
    public void showRecipeList() {

    }

    public bool accept(KeyCode key) {
        throw new System.NotImplementedException();
    }

    public void close() {
        throw new System.NotImplementedException();
    }


    public void open() {
        throw new System.NotImplementedException();
    }
}