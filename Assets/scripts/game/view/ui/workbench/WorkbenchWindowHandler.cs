using game.model.component.building;
using game.view.ui;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

public class WorkbenchWindowHandler : MonoBehaviour, IWindow, IHotKeyAcceptor {
    public TextMeshProUGUI workbenchNameText;
    public Button addOrderButton;
    public GameObject recipesPopup;
    public GameObject orderList;

    // fill wb with recipes and orders
    public void init(EcsEntity entity) {
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        workbenchNameText.text = workbench.name;
        fillOrdersList(workbench);
    }



    // 
    public void showRecipeList() {

    }

    public bool accept(KeyCode key) => false;

    public void close() => gameObject.SetActive(false);


    public void open() => gameObject.SetActive(true);

    public string getName() => "workbench";

    private void fillOrdersList(WorkbenchComponent workbench) {
        
    }
}