using game.model;
using game.model.system;
using TMPro;
using UnityEngine;

namespace game.view.ui.gamespeed_widget {
    public class GameTimeWidgetHandler : MonoBehaviour {
        public RectTransform spinner;
        public TextMeshProUGUI text;

        public void Update() {
            GameTime time = GameModel.get().time;
            text.text = time.year + " " + time.month + " " + time.day + " " + time.hour + ":" + time.minute;
            float rotation = (time.hour * 60 + time.minute) / -4;
            spinner.rotation = Quaternion.Euler(0,0,rotation);  
        }
    }
}