﻿using System;
using game.model.component.plant;
using generation.plant;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    // Some plants start growing their product from a certain point of their growth
    // checks plant growth to start product growth
    // When plant is harvestable, it remains harvestable for a limited time, then starts product growing again
    // checks harvestKeepTime to destroy harvest
    public class PlantWaitingSystem : LocalModelPlantSystem {
        public EcsFilter<PlantProductGrowthWaitingComponent> growthStartFilter;
        public EcsFilter<PlantHarvestKeepComponent> harvestDestroyFilter;
        private readonly PlantGenerator generator = new();

        protected override void runLogic(int updates) {
            foreach (int i in growthStartFilter) {
                PlantProductGrowthWaitingComponent component = growthStartFilter.Get1(i);
                EcsEntity entity = growthStartFilter.GetEntity(i);
                if (entity.take<PlantGrowthComponent>().growth > component.productGrowthStartAbsolute) {
                    entity.Del<PlantProductGrowthWaitingComponent>();
                    entity.Replace(generator.generateProductGrowthComponent(entity.take<PlantComponent>().type, 0));
                }
            }
            foreach (int i in harvestDestroyFilter) {
                ref PlantHarvestKeepComponent component = ref harvestDestroyFilter.Get1(i);
                EcsEntity entity = harvestDestroyFilter.GetEntity(i);
                component.harvestTime += TIME_DELTA * updates;
                if (component.harvestTime > component.productKeepTime) {
                    Debug.Log("plant product kept for max time");
                    entity.Del<PlantHarvestableComponent>();
                    entity.Replace(new PlantHarvestedComponent());
                }
            }
        }
    }
}
