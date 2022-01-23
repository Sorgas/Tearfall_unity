using System.Collections.Generic;
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

        public struct TaskStatusComponent {
            public TaskStatusEnum status;
        }

        // task reopened by TaskReopenSystem
        public struct FailedTaskComponent {
            public int timeout;
        }
        
        // completed or canceled task
        public struct ToRemoveTaskComponent {
            
        }
    }
}