using System.Collections.Generic;
using generation;
using generation.localgen;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace mainMenu {
// Handler for local generation stage. Shows progress bar, messages from localGenerators and counts executed generators.
public class LocalGenerationHandler : GameMenuPanelHandler {
    public Image progressBar;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI counterText;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData>();
    }

    // generates local map based on data saved in GenerationState
    public void startGeneration() {
        GenerationState.get().generateLocalMap("main");
    }

    public new void Update() {
        LocalGenSequence sequence = GenerationState.get().localMapGenerator.localGenSequence;
        if (sequence == null) return;
        counterText.text = $"{sequence.progress} / {sequence.maxProgress}";
        descriptionText.text = sequence.currentMessage;
        float progress = (float)sequence.progress / sequence.maxProgress;
        progressBar.rectTransform.localScale = new Vector3(progress, 0, 0);
        if (sequence.progress == sequence.maxProgress) SceneManager.LoadScene("LocalWorldScene");
    }
}
}