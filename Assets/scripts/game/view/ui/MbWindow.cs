using UnityEngine;

namespace game.view.ui {
    
    // window is UI piece that can be closed and opened
    public class MbWindow : MonoBehaviour, IWindow {
        
        public virtual void close() {
            gameObject.SetActive(false);
        }

        public virtual void open() {
            gameObject.SetActive(true);
        }

        public string getName() {
            return "";
        }
    }
}