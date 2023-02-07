using generation;
using generation.localgen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace mainMenu {
    public class LocalGenerationHandler : MonoBehaviour {
        public UnityEngine.UIElements.Slider slider;
        public Text text;
        
        void Update() {
            LocalGenSequence sequence = GenerationState.get().localMapGenerator.localGenSequence;
            if(sequence == null) return;
            float progress = sequence.progress / sequence.maxProgress;
            slider.SetValueWithoutNotify(progress);
            text.text = sequence.currentMessage;
            if(progress == 1) SceneManager.LoadScene("LocalWorldScene");
        }

        void OnEnable() {
            GenerationState.get().localMapGenerator.generateLocalMap("main", new Vector2Int()); // TODO move to generation stage with progress bar
        }
    }
}