using System.Collections.Generic;
using Assets.scripts.enums.action;
using Tearfall_unity.Assets.scripts.enums;
using UnityEngine;

// stores unit's position, task target position and path to target
public struct MovementComponent {
    public Vector3Int position; // model position
    public Vector3Int target;
    public bool hasTarget;
    public ActionTargetTypeEnum targetType; // near/exact/any
    public List<Vector3Int> path;
    public float speed;
    public float step; // speed is added to this value; when reaches 1, position changed
}

// stores info for drawing unit sprite
public struct VisualMovementComponent {
    public Vector3Int lastPosition; // tracked MovementComponent.position
    public Vector3 position;
    public OrientationEnum orientation;
}

public struct UnitVisualComponent {
    public SpriteRenderer spriteRenderer;
}

public struct TaskComponent {
    public string currentTask;
}

public struct EquipmentComponent {
    public List<string> slots;
}

public struct BodyComponent {
    public List<string> bodyParts;
}

public struct HealthComponent {
    public List<string> injures;
}

public struct JobsComponent {
    public List<string> enabledJobs;
}

public struct OwnershipComponent {
    
}

public struct TestComponent {
    public string value;
}

// stores body temperature
public struct TemperatureComponent {
    public float value;
}

public struct UnitNameComponent {
    public string name;
}

public struct AgeComponent {
    public int age;
}
