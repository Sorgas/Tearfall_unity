using System.Linq;
using game.model.component;
using game.model.component.building;
using game.model.component.task.action;
using game.model.component.task.action.needs;
using game.model.component.unit;
using game.model.localmap;
using game.model.system;
using game.model.system.unit;
using Leopotam.Ecs;
using types.action;
using types.unit.disease;
using UnityEngine;
using util.lang.extension;

// TODO make sleeping restore diminishing fatigue values
namespace types.unit.need {
public class RestNeed : Need {
    private const float hoursToComfort = 16f;
    public const float hoursToHealth = 20f;
    public const float hoursToSafety = 36f; // full need capacity 1 to 0

    private const float comfortThreshold = 1f - hoursToComfort / hoursToSafety;
    private const float healthThreshold = 1f - hoursToHealth / hoursToSafety;
    public const float perTickChange = 1f / hoursToSafety / GameTime.ticksPerHour;

    public override int getPriority(float value) {
        return value switch {
            > comfortThreshold => TaskPriorities.NONE,
            > healthThreshold => TaskPriorities.COMFORT,
            > 0 => TaskPriorities.HEALTH_NEEDS,
            _ => 8
        };
    }

    public override string getStatusEffect(float value) {
        if (value > comfortThreshold) return null;
        if (value > healthThreshold) return "sleepy";
        if (value > 0) return "tired";
        return "exhausted";
    }

    public override Action tryCreateAction(LocalModel model, EcsEntity unit) {
        UnitNeedComponent component = unit.take<UnitNeedComponent>();
        if (component.rest <= 0) {
            return new SleepOnGroundAction(unit.pos());
        }
        switch (component.restPriority) {
            case TaskPriorities.COMFORT:
            case TaskPriorities.HEALTH_NEEDS: { // select bed
                EcsEntity building = findFreeBed(model, unit);
                if (building != EcsEntity.Null) {
                    return new SleepInBedAction(building);
                }
            }
                break;
            case TaskPriorities.SAFETY: {
                Vector3Int placeToSleep = findPlaceUnderRoof(model, unit);
                return new SleepOnGroundAction(placeToSleep);
            }
        }
        return null;
    }

    public override UnitTaskAssignment tryCreateAssignment(LocalModel model, EcsEntity unit) {
        UnitNeedComponent component = unit.take<UnitNeedComponent>();
        if (component.restPriority == TaskPriorities.NONE) return null;
        if (component.rest <= 0) {
            return new UnitTaskAssignment(EcsEntity.Null, unit.pos(), "sleep_ground", unit, component.restPriority);
        }
        Vector3Int unitPos = unit.pos();
        switch (component.restPriority) {
            case TaskPriorities.COMFORT:
            case TaskPriorities.HEALTH_NEEDS: { // select bed
                EcsEntity building = findFreeBed(model, unit);
                if (building != EcsEntity.Null) {
                    return new UnitTaskAssignment(building, building.pos(), "sleep", unit, component.restPriority);
                }
            }
                break;
            case TaskPriorities.SAFETY: {
                Vector3Int placeToSleep = findPlaceUnderRoof(model, unit);
                return new UnitTaskAssignment(EcsEntity.Null, placeToSleep, "sleep_ground", unit, component.restPriority);
            }
        }
        return null;
    }

    public override UnitDisease createDisease() {
        return null;
    }

    private EcsEntity findFreeBed(LocalModel model, EcsEntity unit) {
        Vector3Int unitPos = unit.pos();
        return model.buildingContainer.buildings.Values
            .Where(building => building.Has<BuildingSleepFurnitureC>())
            .Where(building => !building.Has<OwnedComponent>())
            .Where(building => !building.Has<LockedComponent>())
            .OrderBy(building => (unitPos - building.pos()).magnitude)
            .FirstOrDefault();
    }

    // TODO make units look for floor inside and under roof for SAFETY priority
    private Vector3Int findPlaceUnderRoof(LocalModel model, EcsEntity unit) {
        return unit.pos(); // TODO
    }
}
}