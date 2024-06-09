using System;
using Leopotam.Ecs;

namespace types.unit.disease {
// Disease which cause is certain condition of a unit, e.g. 0 hunger. Has conditions to determine should disease progress or heal.
public class ConditionalDisease : Disease {
    public Func<EcsEntity, bool> condition;

    public ConditionalDisease(string name, bool lethal, float hoursToKill,
        string description, string notificationText, string notificationIcon, Func<EcsEntity, bool> condition)
        : base(name, lethal, hoursToKill, description, notificationText, notificationIcon) {
        this.condition = condition;
    }
}
}