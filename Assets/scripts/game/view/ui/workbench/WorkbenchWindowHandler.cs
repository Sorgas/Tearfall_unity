using System.Collections.Generic;
using Assets.scripts.types.item.recipe;
using game.model.component.building;
using game.view.ui;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.building;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

public class WorkbenchWindowHandler : MonoBehaviour, IWindow, IHotKeyAcceptor {
    public TextMeshProUGUI workbenchNameText;
    public Button addOrderButton;
    public GameObject recipePopup;
    public GameObject orderList;

    private EcsEntity entity;

    public void Start() {
        addOrderButton.onClick.AddListener(() => toggleRecipeList());

    }

    // fill wb with recipes and orders
    public void init(EcsEntity entity) {
        WorkbenchComponent workbench = entity.take<WorkbenchComponent>();
        workbenchNameText.text = entity.take<BuildingComponent>().type.name;
        fillOrdersList(workbench);
        fillRecipeList(entity);
    }

    // 

    public bool accept(KeyCode key) => false;

    public void close() => gameObject.SetActive(false);

    public void open() => gameObject.SetActive(true);

    public string getName() => "workbench";

    private void fillOrdersList(WorkbenchComponent workbench) {
        foreach (Transform child in recipePopup.transform) {
           GameObject.Destroy(child.gameObject);
        }
        foreach(CraftingOrder order in workbench.orders) {

        }
    }

    private void toggleRecipeList() {
        recipePopup.gameObject.SetActive(!recipePopup.gameObject.activeSelf);
    }

    private void fillRecipeList(EcsEntity entity) {
        foreach (Transform child in recipePopup.transform) {
           GameObject.Destroy(child.gameObject);
        }
        string name = entity.take<BuildingComponent>().type.name;
        List<string> recipeNames = BuildingTypeMap.get().getRecipes(name);
        for (int i = 0; i < recipeNames.Count; i++) {
            string recipeName = recipeNames[i];
            GameObject line = PrefabLoader.create("recipeLine", recipePopup.transform, new Vector3(0, i * -40, 0));
            line.GetComponent<Button>().onClick.AddListener(() => {
                createOrder(recipeName);
            });
        }
    }

    private void createOrder(string recipeName) {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        Recipe recipe = RecipeMap.get().get(recipeName);
        CraftingOrder order = new(recipe);
        createOrderLine(order);
    }

    private void createOrderLine(CraftingOrder order) {
        ref WorkbenchComponent workbench = ref entity.takeRef<WorkbenchComponent>();
        GameObject line = PrefabLoader.create("craftingOrderLine", orderList.transform,
           new Vector3(0, workbench.orders.Count * -120, 0));
        line.GetComponent<OrderLineHandler>().initForOrder(order);
    }
}