using System;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {

// units should not be assigned to the same task simultaneously.
// Also, two need satisfying tasks targeting same entity should not be created simultaneously;
// used to detect simultaneous creation of multiple tasks targeting one item or building (for eating, sleeping, etc.)
public class TaskAssignmentDescriptor {
    public TaskTargetDescriptor target;
    public TaskPerformerDescriptor performer;

    public TaskAssignmentDescriptor(EcsEntity target, Vector3Int position, string actionType, EcsEntity unit, int priority) {
        this.target = new TaskTargetDescriptor(target, position, actionType);
        performer = new TaskPerformerDescriptor(unit, priority);
    }
}

public class TaskTargetDescriptor : IEquatable<TaskTargetDescriptor> {
    public readonly EcsEntity target; // can be task, item, building or null
    public readonly Vector3Int targetPosition;
    public readonly string actionType; // task, eat, wear, sleep
    
    public TaskTargetDescriptor(EcsEntity target, Vector3Int targetPosition, string actionType) {
        this.target = target;
        this.targetPosition = targetPosition;
        this.actionType = actionType;
    }
    
    public EcsEntity createTask() {
        return EcsEntity.Null;
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
        return Equals((TaskAssignmentDescriptor)obj);
    }
    public override int GetHashCode() {
        return HashCode.Combine(target, targetPosition);
    }
}

public class TaskPerformerDescriptor {
    public readonly EcsEntity unit;
    public readonly int priority;

    public TaskPerformerDescriptor(EcsEntity unit, int priority) {
        this.unit = unit;
        this.priority = priority;
    }
}
}