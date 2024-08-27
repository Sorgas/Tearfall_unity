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
            close();
        }
    }
    
    private void fillForUnit(EcsEntity unit, Vector3Int position) {
        addButton("move", () => GameModel.get().currentLocalModel.addModelAction(model => {
                if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                    if (unit.Has<TaskComponent>()) {
                        GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                    }
                    unit.Replace(new UnitNextTaskComponent { action = new MoveAction(position) });
                }
            })
        );
        List<EcsEntity> usedItems = new ();
        List<EcsEntity> items = GameModel.get().currentLocalModel.itemContainer.onMap.getItems(position);
        foreach (var item in items) {
            if (items.Any(item => item.Has<ItemWeaponComponent>()) && !usedItems.Contains(item)) {
                usedItems.Add(item);
                createEquipButton(item, unit);
            }
        }
        foreach (var item in items) {
            if (items.Any(item => item.Has<ItemToolComponent>()) && !usedItems.Contains(item)) {
                usedItems.Add(item);
                createEquipButton(item, unit);
            }
        }
        foreach (var item in items) {
            if (items.Any(item => item.Has<ItemWearComponent>()) && !usedItems.Contains(item)) {
                usedItems.Add(item);
                createEquipButton(item, unit);
            }
        }
    }

    private void createEquipButton(EcsEntity item, EcsEntity unit) {
        addButton($"equip {item.name()}", () => GameModel.get().currentLocalModel.addModelAction(model => {
                Vector3Int position = model.itemContainer.getItemAccessPosition(item);
                if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                    Action action = getEquipAction(item);
                    if (action != null) {
                        if (unit.Has<TaskComponent>()) {
                            GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                        }
                        unit.Replace(new UnitNextTaskComponent { action = action });
                    }
                }
            })
        );
    }

    private Action getEquipAction(EcsEntity item) {
        if (item.Has<ItemWeaponComponent>() || item.Has<ItemToolComponent>()) {
            return new EquipToolItemAction(item);
        }
        if (item.Has<ItemWearComponent>()) {
            return new EquipWearItemAction(item);
        }
        return null;
    }
    
    private void addButton(string text, System.Action action) {
        GameObject buttonGo = PrefabLoader.create("GenericTextButton", transform, new Vector3(0, -currentIndex * 30, 0));
        buttonGo.GetComponent<Button>().onClick.AddListener(action.Invoke);
        buttonGo.GetComponent<Button>().onClick.AddListener(close);
        buttonGo.GetComponentInChildren<TextMeshProUGUI>().text = text;
        currentIndex++;
    }

    private void clear() {
        currentIndex = 0;
        foreach (Transform o in transform) {
            Destroy(o.gameObject);
        }
    }
    
    private void close() {
        gameObject.SetActive(false);
    }
}
}