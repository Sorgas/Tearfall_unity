using UnityEngine;

namespace game.view {
    public class HotKeyInputSystem {

        public void update() {
            if (Input.GetKeyDown(KeyCode.J)) {
                GameView.get().toggleJobsMenu(); 
            }
        }
    }
}