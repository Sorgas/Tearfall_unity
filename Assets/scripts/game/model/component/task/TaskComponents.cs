using System.Collections.Generic;
using System.Data;
using game.model.component.task.action;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;

namespace game.model.component.task {
    public class TaskComponents {
        public struct TaskActionsComponent {
            public LocalModel model;
            public Action initialAction;
            public List<Action> preActions;

            public Action NextAction => preActions.Count > 0 ? preActions[0] : initialAction;

            public void addFirstPreAction(Action action) => preActions.Insert(0, action);

            public void removeFirstPreAction() {
                if(preActions.Count == 0) {
                    throw new DataException("Trying to remove pre-action when no pre-actions exist"); // this should never happen, See UnitActionCheckingSystem
                }
                preActions.RemoveAt(0);
            }

            public string toString => initialAction.ToString();
        }

        // points to unit. can be with on task or action
        public struct TaskPerformerComponent {
            public EcsEntity performer;
        }

        public struct TaskJobComponent {
            public string job;
        }

        // exists, if task is generated from designation
        public struct TaskDesignationComponent {
            public EcsEntity designation;
        }

        // exists, if task is generated from building(workbench)
        public struct TaskBuildingComponent {
            public EcsEntity building;
        }

        public struct TaskPriorityComponent {
            public TaskPriorityEnum priority;
        }

        // when building blocks, target reachability should be calculated basing on override block type
        public struct TaskBlockOverrideComponent {
            public BlockType blockType;
        }

        // stores items locked for task. See ItemLockedComponent
        public struct TaskLockedItemsComponent {
            public List<EcsEntity> lockedItems;
        }       
    }
}