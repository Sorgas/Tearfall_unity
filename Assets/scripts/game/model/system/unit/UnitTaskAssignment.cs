using System;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.needs;
using game.model.localmap;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;
using util.lang.extension;
using Action = game.model.component.task.action.Action;

namespace game.model.system.unit {

// Units should not be assigned to the same task simultaneously.
// Also, two need satisfying tasks targeting same entity should not be created simultaneously (two tasks for eating same piece of food);
// This class combines task and target entity as equatable objects to prevent simultaneous creation of multiple conflicting tasks.
// Also, stores unit and priority for selection of best(max prioritized and nearest) performer when conflict occurs. 
public class UnitTaskAssignment {
    public TaskTargetDescriptor target;
    public TaskPerformerDescriptor performer;

    public UnitTaskAssignment(EcsEntity target, Vector3Int position, string actionType, EcsEntity unit, int priority) {
        this.target = new TaskTargetDescriptor(target, position, actionType);
        performer = new TaskPerformerDescriptor(unit, priority);
    }

    public override string ToString() => $"{target}-{performer}";
}

public class TaskTargetDescriptor : IEquatable<TaskTargetDescriptor> {
    private readonly EcsEntity target; // can be task, item, building or null
    public readonly Vector3Int targetPosition;
    private readonly string actionType; // task, eat, wear, sleep
    
    public TaskTargetDescriptor(EcsEntity target, Vector3Int targetPosition, string actionType) {
        this.target = target;
        this.targetPosition = targetPosition;
        this.actionType = actionType;
    }
    
    public EcsEntity createTask(TaskPerformerDescriptor performer, LocalModel model) {
        return actionType switch {
            "task" => target,
            "eat" => createNeedTask(new EatAction(target), performer.priority, model),
            "sleep" => createNeedTask(new SleepInBedAction(target), performer.priority, model),
            "sleep_ground" => createNeedTask(new SleepOnGroundAction(targetPosition), performer.priority, model),
            "wear" => createNeedTask(new EquipWearItemAction(target), performer.priority, model),
            _ => EcsEntity.Null
        };
    }

    private EcsEntity createNeedTask(Action action, int priority, LocalModel model) {
        return model.taskContainer.generator.createTask(action, Jobs.NONE, priority, model.createEntity(), model);
    }
    
    public bool Equals(TaskTargetDescriptor other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return target.Equals(other.target) && targetPosition.Equals(other.targetPosition);
    }
    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UnitTaskAssignment)obj);
    }
    public override int GetHashCode() {
        return HashCode.Combine(target, targetPosition);
    }

    public override string ToString() => $"{actionType}: {target} {targetPosition}";
}

public class TaskPerformerDescriptor {
    public readonly EcsEntity unit;
    public readonly int priority;

    public TaskPerformerDescriptor(EcsEntity unit, int priority) {
        this.unit = unit;
        this.priority = priority;
    }

    public override string ToString() => $"{unit.name()} {priority}";
}
}