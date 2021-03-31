using System.Collections;
using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.mainMenu.worldmap;
using Tearfall_unity.Assets.scripts.game.view;
using UnityEngine;

// system for controlling camera on local map;
public class LocalMapCameraSystem : MonoBehaviour {
    public Camera camera;
    private LocalMapCameraController controller;

    void Start() {
        controller = new LocalMapCameraController(camera, GameModel.get().localMap.xSize);
    }

    void Update() {
        if(controller != null) controller.handleInput();;
    }
}
