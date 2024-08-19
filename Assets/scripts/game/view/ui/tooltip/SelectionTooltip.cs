using System.Collections.Generic;
using System.Linq;
using game.model;
using game.model.component;
using game.model.component.item;
using game.model.component.task.action;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types;
using types.action;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using Action = System.Action;

namespace game.view.ui.tooltip {
// Shown when player clicks rmb while SelectionMouseTool is used.
public class SelectionTooltip : MonoBehaviour {
    private RectTransform self;
    
    public void showForEntity(EcsEntity selectedEntity, Vector3Int position) {
        clear();
        Debug.Log(Input.mousePosition);
        transform.localPosition = Input.mousePosition + new Vector3(- 10, 10, 0);
        Debug.Log(transform.localPosition);
        if (selectedEntity.Has<UnitComponent>()) {
            fillForUnit(selectedEntity, position);
        }
        
        gameObject.SetActive(true);
    }

    public void Awake() {
        self = gameObject.GetComponent<RectTransform>();
    }
    
    public void Update() {
        if (!self.rect.Contains(self.InverseTransformPoint(Input.mousePosition))) {
            // close();
        }
    }
    
    private void fillForUnit(EcsEntity unit, Vector3Int position) {
        addButton(0, "move", () => GameModel.get().currentLocalModel.addModelAction(model => {
                if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                    if (unit.Has<TaskComponent>()) {
                        GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                    }
                    unit.Replace(new UnitNextTaskComponent { action = new MoveAction(position) });
                }
            })
        );
        // List<EcsEntity> items = GameModel.get().currentLocalModel.itemContainer.onMap.getItems(position);
        // if (items.Any(item => item.Has<ItemWearComponent>())) {
        //     
        // }
    }

    private void addButton(int index, string text, Action action) {
        GameObject buttonGo = PrefabLoader.create("GenericTextButton", transform, new Vector3(0, -index * 30, 0));
        buttonGo.GetComponent<Button>().onClick.AddListener(action.Invoke);
        buttonGo.GetComponent<Button>().onClick.AddListener(close);
        buttonGo.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    private void clear() {
        foreach (Transform o in transform) {
            Destroy(o.gameObject);
        }
    }
    
    private void close() {
        gameObject.SetActive(false);
    }
}
}