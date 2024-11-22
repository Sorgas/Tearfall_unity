﻿using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.component.task.order;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using types.building;
using types.material;
using UnityEngine;
using util.lang.extension;
using static UnityEngine.Object;

namespace game.model.container {
    // registry of designation entities, creates and destroys designations, called by ECS systems
    // TODO allow multiple designation on one tile
    public class DesignationContainer : LocalModelContainer {
        public readonly Dictionary<Vector3Int, EcsEntity> designations = new();

        public DesignationContainer(LocalModel model) : base(model) { }

        public void createDesignation(Vector3Int position, DesignationType type, int priority) {
            EcsEntity entity = model.createEntity();
            entity.Replace(new DesignationComponent { type = type, priority = priority});
            addDesignation(entity, position);
            Debug.Log("[DesignationContainer] designation created " + position);
        }

        public void createConstructionDesignation(Vector3Int position, ConstructionOrder order, int priority) {
            EcsEntity entity = model.createEntity();
            ConstructionOrder newOrder = new ConstructionOrder(order);
            newOrder.position = position;
            entity.Replace(new DesignationComponent { type = DesignationTypes.D_CONSTRUCT, priority = priority });
            entity.Replace(new DesignationConstructionComponent { order = newOrder });
            entity.Replace(new ItemContainerComponent { items = new List<EcsEntity>() });
            addDesignation(entity, position);
            Debug.Log("Construction designation created " + position);
        }
        
        public void createBuildingDesignation(Vector3Int position, Orientations orientation, BuildingOrder order, int priority) {
            EcsEntity entity = model.createEntity();
            BuildingOrder newOrder = new BuildingOrder(order);
            newOrder.position = position;
            newOrder.orientation = orientation;
            entity.Replace(new DesignationComponent { type = DesignationTypes.D_BUILD, priority = priority});
            entity.Replace(new DesignationBuildingComponent { order = newOrder });
            entity.Replace(new ItemContainerComponent { items = new List<EcsEntity>() }); // designation will store material items before building (OnI)
            addDesignation(entity, position);
            Debug.Log("Construction designation created " + position);
        }
            
        private void addDesignation(EcsEntity entity, Vector3Int position) {
            entity.Replace(new PositionComponent { position = position });
            removeDesignation(position); // replace previous designation
            // TODO
            // if (entity.Has<MultiPositionComponent>()) {
            //     foreach (Vector3Int position in entity.Get<MultiPositionComponent>().positions) {
            //
            //     }
            // }
            designations[position] = entity;
        }

        // removes designation in given position. if it had task, removes it too
        public void removeDesignation(Vector3Int position) {
            if (!designations.ContainsKey(position)) return;
            EcsEntity designation = designations[position];
            removeDesignationVisual(designations[position]);
            designations.Remove(designation.pos());
            removeDesignationTask(designation);
            designation.Destroy();
            Debug.Log("Designation removed " + position);
        }

        private void removeDesignationTask(EcsEntity designation) {
            if (designation.Has<TaskComponent>()) {
                EcsEntity task = designation.take<TaskComponent>().task;
                task.Del<TaskDesignationComponent>();
                designation.Del<TaskComponent>();
                model.taskContainer.removeTask(task, TaskStatusEnum.CANCELED);
            }
        }

        // use only when task is completed! 
        private void removeDesignationVisual(EcsEntity designation) {
            if (designation.Has<DesignationVisualComponent>()) {
                Destroy(designation.Get<DesignationVisualComponent>().spriteRenderer.gameObject);
                designation.Del<DesignationVisualComponent>();
            }
        }

        // private MultiPositionComponent createMultiPositionComponent(BuildingType type, Orientations orientation, Vector3Int position) {
        //     MultiPositionComponent component = new() { positions = new List<Vector3Int>() };
        //     for (int x = 0; x < type.size[0]; x++) {
        //         for (int y = 0; y < type.size[1]; y++) {
        //             component.positions.Add(new Vector3Int(position.x + x, position.y + y, position.z));
        //         }
        //     }
        //     return component;
        // }
    }
}