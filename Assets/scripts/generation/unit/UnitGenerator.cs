using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;
using Random = System.Random;

namespace generation.unit {
class UnitGenerator {
    private UnitEquipmentComponentGenerator equipmentGenerator;
    private UnitBodyComponentGenerator bodyGenerator;
    private UnitNameGenerator nameGenerator;
    private UnitNeedComponentGenerator needGenerator;
    private UnitSkillsGenerator skillsGenerator;
    private Random random;
    private bool debug = false;

    public UnitGenerator(int seed) {
        random = new Random(seed);
        equipmentGenerator = new(random);
        bodyGenerator = new(random);
        nameGenerator = new(random);
        needGenerator = new(random);
        skillsGenerator = new(random);
    }

    // generates units from settler data on game start or migration
    public void generateUnit(UnitData data, EcsEntity entity) {
        CreatureType type = CreatureTypeMap.getType(data.type);
        addCommonComponents(ref entity, data, type);
        addOptionalComponents(ref entity, type);
        addSettlerComponents(ref entity);
        entity.Replace(new FactionComponent { name = data.faction });
    }

    // TODO
    public void generateUnit(string creatureType) {
        CreatureType type = CreatureTypeMap.getType(creatureType);
        type.components.Contains("equipment");
    }

    private void addCommonComponents(ref EcsEntity entity, UnitData data, CreatureType type) {
        entity
            .Replace(nameGenerator.generate(data))
            .Replace(createPropertiesComponent(data))
            .Replace(new UnitVisualComponent { headVariant = data.headVariant, bodyVariant = data.bodyVariant }) // sprite go is created in UnitVisualSystem
            .Replace(new PositionComponent { position = new Vector3Int() })
            .Replace(bodyGenerator.generate(type))
            .Replace(new UnitHealthComponent { overallStatus = "healthy" , stamina = 1f, injuries = new()})
            .Replace(new UnitStatusEffectsComponent { effects = new() })
            .Replace(new OwnershipComponent { wealthStatus = "poor" })
            .Replace(new UnitComponent { type = type })
            .Replace(new AgeComponent { age = data.age });
        needGenerator.generate(ref entity, type);
    }

    private void addSettlerComponents(ref EcsEntity entity) {
        UnitJobsComponent jobsComponent = new UnitJobsComponent { enabledJobs = new() };
        entity.Replace(jobsComponent);
        foreach (var job in Jobs.jobs) {
            jobsComponent.enabledJobs.Add(job.name, 0);
        }
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

    // TODO use default values from race
    private UnitPropertiesComponent createPropertiesComponent(UnitData data) {
        UnitPropertiesComponent component = new UnitPropertiesComponent {
            attributes = new(),
            properties = new()
        };
        component.attributes[UnitAttributes.STRENGTH] = new UnitIntProperty(data.statsData.strength);
        component.attributes[UnitAttributes.AGILITY] = new UnitIntProperty(data.statsData.agility);
        component.attributes[UnitAttributes.ENDURANCE] = new UnitIntProperty(data.statsData.endurance);
        component.attributes[UnitAttributes.INTELLIGENCE] = new UnitIntProperty(data.statsData.intelligence);
        component.attributes[UnitAttributes.SPIRIT] = new UnitIntProperty(data.statsData.spirit);
        component.attributes[UnitAttributes.CHARISMA] = new UnitIntProperty(data.statsData.charisma);
        foreach (UnitProperty property in UnitProperties.ALL) {
            component.properties.Add(property.name, new UnitFloatProperty(property.baseValue));
        }
        return component;
    }
}
}