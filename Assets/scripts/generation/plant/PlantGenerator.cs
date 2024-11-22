using System;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;
using static game.model.component.plant.PlantUpdateType;

namespace generation.plant {
public class PlantGenerator : AbstractGenerator {

    public PlantGenerator(int seed) : base(seed) { }

    public PlantGenerator() { }

    // TODO add age
    public EcsEntity generate(string typeName, float age, EcsEntity entity) {
        PlantType type = PlantTypeMap.get().get(typeName);
        if (type == null) throw new ArgumentException("type " + typeName + " not found in PlantTypeMap. Check resources/data/plants");
        entity.Replace(new PlantComponent { block = createPlantBlock(entity, type), type = type });
        entity.Replace(new NameComponent { name = type.title });
        entity.Get<PlantVisualUpdateComponent>().add(NEW);
        addAgeComponents(entity, age, type);
        return entity;
    }

    // TODO add multi-block tree generator
    public EcsEntity generate(string typeName, EcsEntity entity) {
        return generate(typeName, 0f, entity);
    }

    public EcsEntity generateFromSeed(EcsEntity seedItem, Vector3Int position, EcsEntity entity) {
        string plantName = seedItem.take<ItemSeedComponent>().plant;
        return generate(plantName, position, entity);
    }

    private EcsEntity generate(string typeName, Vector3Int position, EcsEntity entity) {
        generate(typeName, entity);
        entity.Replace(new PositionComponent { position = position });
        return entity;
    }

    private PlantGrowthComponent generateGrowthComponent(PlantType type, float age) {
        if (age > type.maturityAge) throw new ArgumentException("Age cannot be more than maturity age.");
        int currentStage = type.getStageByAge(age);
        // Debug.Log("current stage " + currentStage);
        return new PlantGrowthComponent {
            growth = age,
            maturityAge = type.maturityAge,
            currentStage = currentStage,
            nextStage = type.growthStages[currentStage]
        };
    }

    // age is relative to product growth period
    public PlantProductGrowthComponent generateProductGrowthComponent(PlantType type, float age) {
        return new PlantProductGrowthComponent {
            growth = age,
            growthEnd = type.productGrowthTime
        };
    }

    private PlantBlock createPlantBlock(EcsEntity entity, PlantType type) {
        PlantBlock block = new(type.materialId, PlantBlockTypeEnum.TRUNK.code);
        block.plant = entity;
        return block;
    }

    private void addAgeComponents(EcsEntity entity, float age, PlantType type) {
        entity.Replace(new PlantAgeComponent { age = age, maxAge = type.maxAge });
        if (age < type.maturityAge) { // plant is still growing
            entity.Replace(generateGrowthComponent(type, age));
        }
        if (type.product == null) return;
        if (age < type.productGrowthStartAbsolute) { // plant not yet grows products
            entity.Replace(new PlantProductGrowthWaitingComponent { productGrowthStartAbsolute = type.productGrowthStartAbsolute });
            return;
        }

        // plant grows products
        float normalizedAge = age - type.productGrowthStartAbsolute; // after product growth started
        if (!type.destroyOnHarvest && type.productKeepTime >= 0) // multiple harvests with limited keep time
            normalizedAge %= type.productGrowthTime + type.productKeepTime;
        if (normalizedAge < type.productGrowthTime) { // product is growing
            entity.Replace(generateProductGrowthComponent(type, normalizedAge));
        } else { // product has grown
            entity.Replace(new PlantHarvestableComponent());
            if (type.productKeepTime >= 0) { // limited harvest keep
                entity.Replace(new PlantHarvestKeepComponent { harvestTime = normalizedAge - type.productGrowthTime });
            }
            // single harvest, limited keep time and plant too old
            if (type.destroyOnHarvest && type.productKeepTime >= 0 && normalizedAge > type.productGrowthTime + type.productKeepTime) {
                Debug.LogError("Invalid age " + age + " passed for generation of plant " + type.name);
            }
        }
    }
}
}