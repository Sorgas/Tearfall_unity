using UnityEngine;

namespace game.view.ui.util {
    
    // window is UI piece that can be closed and opened
    public abstract class MbWindow : MonoBehaviour, ICloseable {
        
        public virtual void close() {
            gameObject.SetActive(false);
        }

        public virtual void open() {
            gameObject.SetActive(true);
        }
    }
}