using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;

// Declares custom classes for IEcsRunSystem. They are treated differently regarding game speed for performance purposes.
namespace game.model.system {

// Triggered each engine update per number of elapsed tics.
public abstract class LocalModelUnscalableEcsSystem : EcsModelSystem { }

// Runs independently from game speed. Triggered once per engine update.
public abstract class LocalModelTimeIndependentEcsSystem : EcsModelSystem { }

// Triggered once per engine update if there were elapsed ticks. Should scale its effects to number of elapsed ticks
public abstract class LocalModelScalableEcsSystem : EcsModelSystem {
    public override void Run() => runLogic(globalSharedData.ticks);

    protected abstract void runLogic(int ticks);
}

// Implementation of scalable system, which runs its logic each [interval] ticks elapsed.
public abstract class LocalModelIntervalEcsSystem : LocalModelScalableEcsSystem {
    private int counter;
    private readonly int interval; // in ticks

    protected LocalModelIntervalEcsSystem(int interval) {
        this.interval = interval;
    }

    public override void Run() {
        int updates = rollTimer(globalSharedData.ticks);
        if (updates > 0) runLogic(updates);
    }

    private int rollTimer(int ticks) {
        counter += ticks;
        int updates = counter / interval;
        counter %= interval;
        return updates;
    }
}

// game local model-aware system with logging utility methods. 
public abstract class EcsModelSystem : IEcsRunSystem {
    // injected
    protected EcsGlobalSharedData globalSharedData;
    protected LocalModel model;

    protected string name = "DefaultSystem";
    protected bool debug = false;

    public abstract void Run();

    protected void log(string message) {
        if (debug) Debug.Log($"[{name}]: {message}");
    }

    protected void logWarn(string message) {
        Debug.LogWarning($"[{name}]: {message}");
    }

    protected void logError(string message) {
        Debug.LogError($"[{name}]: {message}");
    }
}

// injected to all systems of GameModel
public class EcsGlobalSharedData {
    public int ticks { get; private set; }

    public void set(int value) => ticks = value;
}
}