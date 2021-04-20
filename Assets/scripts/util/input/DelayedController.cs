using System;
using UnityEngine;

// translates continuous input into sequence of action calls with specified delays.
// Each delay except the last one is used only once. 
// Passes delta time is accumulated. 
// When accumulated time value reaches value of current delay, time is reset, and action is called.
public class DelayedController {
    private Action action;
    protected float[] delays = { 0, 0.3f, 0.08f }; // first 0 means first call always invokes action
    private int currentDelay = 0;
    private float time = 0f; // time from last activation

    public DelayedController(Action action) {
        this.action = action;
    }

    // meant to be called continuously with random intervals
    public void call(float deltaTime) {
        float delay = delays[currentDelay];
        time += deltaTime; // accumulate time
        if (time > delay) { // enough time accumulated
            time -= delay; // keep extra time
            action.Invoke();
            if(currentDelay < delays.Length - 1) currentDelay++; // switch to next delay, if there is one
        }
    }

    public void reset() {
        time = 0;
        currentDelay = 0;
    }
}