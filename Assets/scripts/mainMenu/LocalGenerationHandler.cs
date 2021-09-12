using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tearfall_unity.Assets.scripts.mainMenu {
    public class LocalGenerationHandler : MonoBehaviour {
        public Slider slider;
        public Text text;
        
        void Update() {
            LocalGenSequence sequence = GenerationState.get().localGenSequence;
            if(sequence == null) return;
            float progress = sequence.progress / sequence.maxProgress;
            slider.SetValueWithoutNotify(progress);
            text.text = sequence.currentMessage;
            if(progress == 1) SceneManager.LoadScene("LocalWorldScene");
        }

        void OnEnable() {
            GenerationState.get().generateLocalMap(); // TODO move to generation stage with progress bar
        }
    }
}