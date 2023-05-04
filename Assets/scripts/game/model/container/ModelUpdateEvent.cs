using System;
using game.model.localmap;

namespace game.model.container {
    // generated in mouse tools and consumed in ModelUpdateSystem
    public class ModelUpdateEvent {
        public Action<LocalModel> action;

        public ModelUpdateEvent(Action<LocalModel> action) {
            this.action = action;
        }
    }
}