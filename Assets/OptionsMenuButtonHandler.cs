﻿using System.Collections;
using System.Collections.Generic;
using Assets.scripts.mainMenu;
using UnityEngine;

namespace Assets
{
public class OptionsMenuButtonHandler : ButtonHandler {
    public GameObject mainMenuStage;

    protected override void initButtons() {
        buttons = new List<ButtonData> {
            new ButtonData("ApplyButton", KeyCode.E, () => {
                // todo apply changes
                switchTo(mainMenuStage);
            }),
            new ButtonData("BackButton", KeyCode.Q, () => switchTo(mainMenuStage))
        };
    }
}
}