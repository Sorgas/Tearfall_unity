using System.Linq;
using enums.action;
using enums.unit.need;
using game.model.component;
using game.model.component.building;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

// TODO make sleeping restore diminishing fatigue values
public class RestNeed : Need {
    public const int hoursToComfort = 8;
    public const int hoursToHealth = 16;
    public const float hoursToSafety = 36f; // full need capacity 1 to 0

    public const float comfortThreshold = 1 - hoursToComfort / hoursToSafety;
    public const float healthThreshold = 1 - hoursToHealth / hoursToSafety;

    public override TaskPriorityEnum getPriority(float value) {
        // if (value > comfortThreshold) return TaskPriorityEnum.NONE;
        if (value > healthThreshold) return TaskPriorityEnum.NONE;
        if (value > 0) return TaskPriorityEnum.HEALTH_NEEDS;
        return TaskPriorityEnum.SAFETY;
    }

    public Action tryCreateAction(LocalModel model, EcsEntity unit) {
        UnitNeedComponent component = unit.take<UnitNeedComponent>();
        if (component.rest <= 0) {
            return new SleepOnGroundAction(unit.pos());
        }
        Vector3Int unitPos = unit.pos();
        switch (component.restPriority) {
            case TaskPriorityEnum.COMFORT:
            case TaskPriorityEnum.HEALTH_NEEDS: { // select bed
                    EcsEntity building = findFreeBed(model, unit);
                    if (building != EcsEntity.Null) {
                        return new SleepInBedAction(building);
                    }
                }
                break;
            case TaskPriorityEnum.SAFETY: {
                    Vector3Int placeToSleep = findPlaceUnderRoof(model, unit);
                    return new SleepOnGroundAction(placeToSleep);
                }
        }
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

    public float getSleepSpeedByBedQuality(QualityEnum quality) {
        switch(quality) {
            case QualityEnum.AWFUL : return 0.9f;
            case QualityEnum.BAD : return 0.95f;
            case QualityEnum.NORMAL : return 1;
            case QualityEnum.FINE : return 1.03f;
            case QualityEnum.EXCELLENT : return 1.06f;
            case QualityEnum.MASTERWORK : return 1.09f;
            case QualityEnum.LEGENDARY : return 1.12f;
        }
        Debug.LogWarning("[RestNeed]: unknown quality value!");
        return 0.9f;
    }
}