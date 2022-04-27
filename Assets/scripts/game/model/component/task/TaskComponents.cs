using System.Collections.Generic;
using enums;
using enums.action;
using game.model.component.task.action;
using Leopotam.Ecs;

namespace game.model.component.task {
    public class TaskComponents {

        public struct TaskActionsComponent {
            public Action initialAction;
            public List<Action> preActions;

            public Action getNextAction() {
                return preActions.Count > 0 ? preActions[0] : initialAction;
            }

            public void addFirstPreAction(Action action) {
                preActions.Insert(0, action);
            }

            public void removeFirstPreAction() {
                preActions.RemoveAt(0);
            }

            public string toString() {
                return initialAction.ToString();
            }
        }

        // points to unit. can be with on task or action
        public struct TaskPerformerComponent {
            public EcsEntity performer;
        }

        public struct TaskJobComponent {
            public string job;
        }

        // task can be taken by TaskAssignmentSystem
        public struct OpenTaskComponent {

        }

        // exists, if task is generated from designation
        public struct TaskDesignationComponent {
            public EcsEntity designation;
        }

        public struct TaskPriorityComponent {
            public TaskPriorityEnum priority;
        }

        // when building blocks, target reachability should be calculated basing on override block type
        public struct TaskBlockOverrideComponent {
            public BlockType blockType;
        }
    }
}