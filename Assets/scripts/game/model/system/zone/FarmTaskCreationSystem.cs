using System.Linq;
using game.model.component;
using game.model.component.plant;
using game.model.component.task.action.plant;
using game.model.container;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using Action = game.model.component.task.action.Action;

namespace game.model.system.zone {
    public class FarmTaskCreationSystem : LocalModelEcsSystem {
        public EcsFilter<FarmComponent>.Exclude<FarmOpenHoeingTaskComponent> hoeingFilter;
        private readonly TaskGenerator generator = new();

        public FarmTaskCreationSystem(LocalModel model) : base(model) {
        }

        public override void Run() {
            foreach (int i in hoeingFilter) {
                EcsEntity entity = hoeingFilter.GetEntity(i);
                FarmComponent farm = hoeingFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                FarmTileTrackingComponent tracking = entity.take<FarmTileTrackingComponent>();
                tryCreateHoeingTask(zone, entity);
            }
        }

        private void tryCreateHoeingTask(ZoneComponent zone, EcsEntity entity) {
            FarmTileTrackingComponent tracking = entity.take<FarmTileTrackingComponent>();
            if (tracking.toHoe.Count > 0) { // has tiles to hoe
                Action action = new FarmHoeingAction(entity);
                EcsEntity task = generator.createTask(action, model.createEntity(), model);
                entity.Replace(new FarmOpenHoeingTaskComponent { hoeTask = task });
                model.taskContainer.addOpenTask(task);
            }
        }

        private Action tryCreateTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {

            if (hasUnplantedTiles(zone, farm)) {
                // planting task
            }
            return null;
        }

        private bool hasUnplantedTiles(ZoneComponent zone, FarmComponent farm) {
            if (farm.config.Count == 0) return false;
            return zone.tiles
                .Where(tile => model.localMap.blockType.get(tile) == BlockTypes.FARM.CODE)
                .Where(tile => {
                    EcsEntity plant = model.plantContainer.getPlant(tile);
                    if (plant == EcsEntity.Null) return false;
                    return farm.config.Contains(plant.take<PlantComponent>().type.name);
                })
                .Any();
        }
    }
}