using System.Collections.Generic;
using System.Linq;
using game.model;
using game.model.component;
using game.model.component.item;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
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
    private int currentIndex = 0;
    
    public void showForEntity(EcsEntity selectedEntity, Vector3Int position) {
        clear();
        Debug.Log(Input.mousePosition);
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition, null, out canvasPosition);
        transform.localPosition = new Vector3(canvasPosition.x - 10, canvasPosition.y + 10, 0);
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
        List<EcsEntity> items = GameModel.get().currentLocalModel.itemContainer.onMap.getItems(position);
        foreach (var item in items) {
            if (items.Any(item => item.Has<ItemWearComponent>())) {
                addButton(0, $"equip {item.name()}", () => GameModel.get().currentLocalModel.addModelAction(model => {
                        if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                            if (unit.Has<TaskComponent>()) {
                                GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                            }
                            unit.Replace(new UnitNextTaskComponent { action = new EquipWearItemAction(item) });
                        }
                    })
                );
            }
        }
    }

    private void addButton(int index, string text, Action action) {
        GameObject buttonGo = PrefabLoader.create("GenericTextButton", transform, new Vector3(0, -index * 30, 0));
        buttonGo.GetComponent<Button>().onClick.AddListener(action.Invoke);
        buttonGo.GetComponent<Button>().onClick.AddListener(close);
        buttonGo.GetComponentInChildren<TextMeshProUGUI>().text = text;
        index++;
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