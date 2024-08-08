using System.Collections.Generic;
using generation;
using generation.localgen;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace mainMenu {
// Handler for local generation stage. Shows progress bar, messages from localGenerators and counts executed generators.
// Should be shown during localMap generation
public class LocalGenerationHandler : GameMenuPanelHandler {
    public Image progressBar;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI counterText;
    private int previousProgress = -1;
    
    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData>();
    }
    
    // Updates state of generation progress bar. Values taken from singleton GenerationState
    // Loads next scene when generation completes
    public new void Update() {
        LocalGenSequence sequence = GenerationState.get().localMapGenerator.localGenSequence;
        if (sequence == null) return;
        if (sequence.progress != previousProgress) {
            counterText.text = $"{sequence.progress} / {sequence.maxProgress}";
            descriptionText.text = sequence.currentMessage;
            float progress = (float)sequence.progress / sequence.maxProgress;
            Debug.Log($"progress {progress}");
            progressBar.rectTransform.localScale = new Vector3(progress, 0, 0);
            previousProgress = sequence.progress;
        }
        if (sequence.progress == sequence.maxProgress) SceneManager.LoadScene("LocalWorldScene");
    }
}
}