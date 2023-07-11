using System.Collections.Generic;
using game.model.component.task.action;
using game.model.component.task.order;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using util;

namespace game.model.component.task {
    // main component of tasks
    public struct TaskActionsComponent {
        public LocalModel model;
        public Action initialAction; // main action
        public List<Action> preActions; // actions can create pre-actions
        public Action nextAction => preActions.Count > 0 ? preActions[0] : initialAction;

        public void addFirstPreAction(Action action) => preActions.Insert(0, action);
        
        public void removeFirstPreAction() {
            if(preActions.Count == 0) {
                throw new GameException("Trying to remove pre-action when no pre-actions exist"); // this should never happen, See UnitActionCheckingSystem
            }
            preActions.RemoveAt(0);
        }
    }

    // points to unit who preforms task
    public struct TaskPerformerComponent {
        public EcsEntity performer;
    }

    // unit should have job enabled to get task (optional)
    // tasks are given by priority
    public struct TaskJobComponent {
        public string job;
        public int priority;
    }

    // exists, if task is generated from designation
    public struct TaskDesignationComponent {
        public EcsEntity designation;
    }

    // exists, if task is generated from building(workbench)
    public struct TaskBuildingComponent {
        public EcsEntity building;
    }

    // exists, if task is generated from zone(stockpile, etc.)
    public struct TaskZoneComponent {
        public EcsEntity zone;
        public string taskType; // used for task tracking by zone. see ZoneTaskTypes
    }
    
    // when building blocks, target reachability should be calculated basing on override block type
    public struct TaskBlockOverrideComponent {
        public BlockType blockType;
    }

    // stores items locked for task. See ItemLockedComponent
    public struct TaskLockedEntitiesComponent {
        public List<EcsEntity> entities;
    }

    public struct TaskTimeoutComponent {
        public int timeout;
    }

public struct TaskCraftingOrderComponent {
    public CraftingOrder order;
}
}