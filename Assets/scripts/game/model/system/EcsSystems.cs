using game.model.localmap;
using Leopotam.Ecs;

namespace game.model.system {
    // can emulate faster game speed by multiplying changes to number of emulated ticks
    public abstract class LocalModelScalableEcsSystem : LocalModelUnscalableEcsSystem {

        // ticks represent game speed
        protected abstract void runLogic(int ticks);

        public override sealed void Run() => runLogic(globalSharedData.ticks);
    }

    // EcsSystem that have link to LocalMapModel for operating on entities
    // cannot emulate faster game speed, called multiple times from game model
    public abstract class LocalModelUnscalableEcsSystem : MyEcsRunSystem {
        protected EcsGlobalSharedData globalSharedData; // injected
        protected LocalModel model; // injected
        
        public override abstract void Run();
    }
    
    public abstract class LocalModelIntervalEcsSystem : LocalModelScalableEcsSystem {
        private int counter;
        private readonly int interval; // in ticks

        protected LocalModelIntervalEcsSystem(int interval) {
            this.interval = interval;
        }
        
        protected abstract void runIntervalLogic(int updates);

        protected override sealed void runLogic(int ticks) {
            int updates = rollTimer(ticks); 
            if(updates > 0) runIntervalLogic(updates);
        }
        
        private int rollTimer(int ticks) {
            counter += ticks;
            int updates = counter / interval;
            counter %= interval;
            return updates;
        }
    }
    
    // just to make Run() virtual
    public class MyEcsRunSystem : IEcsRunSystem {
        public virtual void Run() { }
    }
}