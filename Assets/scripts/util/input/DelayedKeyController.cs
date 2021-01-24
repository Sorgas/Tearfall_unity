using System;
using System.Collections;
using UnityEngine;

// hides some input events to emulate continuous typing a key.
public class DelayedKeyController {
    private KeyCode keycode;
    private Action action;
    private float delay1 = 0.4f;
    private float delay2 = 0.1f;
    private float time = 0f; // time from last activation
    private bool active = false;
    private bool firstDelay = true;

    public DelayedKeyController(KeyCode keycode, Action action) {
        this.keycode = keycode;
        this.action = action;
    }

    public void update(float deltaTime) {
        if (Input.GetKeyUp(keycode)) {
            time = 0;
            active = false;
            firstDelay = true;
            return;
        }
        if (Input.GetKeyDown(keycode)) {
            active = true;
            action.Invoke();
            return;
        }
        if (active && Input.GetKey(keycode)) {
            time += deltaTime;
            if(!firstDelay) checkDelay(delay2);
            if (firstDelay && checkDelay(delay1)) {
                firstDelay = false;
                return;
            } 
        }
    }

    public bool checkDelay(float delay) {
        if (time > delay) {
            action.Invoke();
            time -= delay; // keep extra time
            return true;
        }
        return false;
    }
}