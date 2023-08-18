using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using util.item;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action {
// action that requires performer to have tool.
public abstract class ToolAction : EquipmentAction {
    protected readonly string toolAction;
    protected readonly ToolWithActionItemSelector toolSelector;

    protected ToolAction(string toolAction, ActionTarget target) : base(target) {
        this.toolAction = toolAction;
        toolSelector = new ToolWithActionItemSelector(toolAction);

        assignmentCondition = unit => {
            if (unit.take<UnitEquipmentComponent>().toolWithActionEquipped(toolAction)) return OK;
            EcsEntity item = container.findingUtil.findItemBySelector(toolSelector, unit.pos());
            return item != EcsEntity.Null ? OK : FAIL;
        };
    }
}
}