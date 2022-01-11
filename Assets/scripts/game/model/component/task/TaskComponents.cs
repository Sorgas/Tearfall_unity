using System.Collections.Generic;
using enums.action;
using game.model.component.task.action;
using Leopotam.Ecs;

namespace game.model.component.task {
    public class TaskComponents {
        // displayed name of a task
        public struct TaskNameComponent {
            public string name;
        }
        
        public struct TaskActionsComponent {
            public Action initialAction;
            public List<Action> preActions;
            public TaskStatusEnum status;
            
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

        public struct OpenTaskComponent {
            
        }
    }
}