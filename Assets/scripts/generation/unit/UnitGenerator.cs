using System.Collections.Generic;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;
using util.lang.extension;

namespace generation.unit {
class UnitGenerator {
    private UnitEquipmentComponentGenerator equipmentGenerator = new();
    private UnitBodyComponentGenerator bodyGenerator = new();
    private UnitNameGenerator nameGenerator = new();
    private UnitNeedComponentGenerator needGenerator = new();
    private UnitSkillsGenerator skillsGenerator = new();
    private bool debug = false;

    // generates units from settler data on game start or migration
    public void generateUnit(SettlerData data, EcsEntity entity) {
        CreatureType type = CreatureTypeMap.getType(data.type);
        addCommonComponents(ref entity, data, type);
        addOptionalComponents(ref entity, type);
        addSettlerComponents(ref entity);
    }

    // TODO
    public void generateUnit(string creatureType) {
        CreatureType type = CreatureTypeMap.getType(creatureType);
        type.components.Contains("equipment");
    }

    private void addCommonComponents(ref EcsEntity entity, SettlerData data, CreatureType type) {
        entity
            .Replace(nameGenerator.generate(data))
            .Replace(new UnitMovementComponent { speed = 0.03f, currentSpeed = -1, step = 0 }) // TODO add stat or trait dependency
            .Replace(new UnitVisualComponent { headVariant = data.headVariant, bodyVariant = data.bodyVariant }) // sprite go is created in UnitVisualSystem
            .Replace(new PositionComponent { position = new Vector3Int() })
            .Replace(bodyGenerator.generate(type))
            .Replace(createAttributesComponent(data.statsData))
            .Replace(new HealthComponent { overallStatus = "healthy" })
            .Replace(new MoodComponent { status = "content" })
            .Replace(new OwnershipComponent { wealthStatus = "poor" })
            .Replace(new UnitComponent { type = type })
            .Replace(new AgeComponent { age = data.age });
        needGenerator.generate(ref entity, type);
    }

    private void addSettlerComponents(ref EcsEntity entity) {
        entity.Replace(new UnitJobsComponent { enabledJobs = new() });
        entity.Replace(new UnitNamesComponent { });
        skillsGenerator.generate(entity);
    }

    private void addOptionalComponents(ref EcsEntity entity, CreatureType type) {
        if (type.components.Contains("equipment")) {
            entity.Replace(equipmentGenerator.generate(type));
        }
        if (type.components.Contains("door_user")) {
            entity.Replace(new UnitDoorUserComponent());
        }
    }

    private UnitAttributesComponent createAttributesComponent(StatsData data) {
        return new UnitAttributesComponent {
            strength = data.strength,
            agility = data.agility,
            endurance = data.endurance,
            intelligence = data.intelligence,
            will = data.spirit,
            charisma = data.charisma
        };
    }
}
}