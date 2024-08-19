using game.model;
using game.model.component;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.view.ui.workbench;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
// handles display of item equipped in layer of unit's slot
public class UnitEquipmentSlotLayerHandler : ItemButtonWithTooltipHandler {
    public Button dropButton;
    private bool dropButtonEnabled;
    private EcsEntity unit;
    
    public void Start() {
        dropButton.onClick.AddListener(() => {
            GameModel.get().currentLocalModel.addModelAction(model => {
                if (unit.Has<TaskComponent>()) { // fail current task
                    GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                }
                unit.Replace(new UnitNextTaskComponent { action = new PutItemToPositionAction(item, unit.pos()) });
            });
        });
    }

    public void initFor(EcsEntity unit, EcsEntity item, int amount = -1) {
        base.initFor(item, amount); // displays item
        this.unit = unit;
        // TooltipParentHandler handler = gameObject.GetComponent<TooltipParentHandler>();
        // handler.addTooltipObject(dropButton.gameObject, item != EcsEntity.Null);
            // if (decal.Equals("hand")) {
            //     showDecal(IconLoader.get().getSprite("unit_window/empty_hand"), Color.white);
            // }
            // if (decal.Equals("none")) {
            //     showDecal(null, Color.clear);
            // }
    }
    
    private void showDecal(Sprite sprite, Color color) {
        image.sprite = sprite;
        image.color = color;
    }
}
}