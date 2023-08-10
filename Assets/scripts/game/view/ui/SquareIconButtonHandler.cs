using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace game.view.ui {
public class SquareIconButtonHandler : MonoBehaviour {
    public Image background;
    public Image frame;
    public Image icon;
    public TextMeshProUGUI text;
    public Button button;
    
    public void addListener(UnityAction action) {
        button.onClick.AddListener(action);
    }

    public void setColor(Color color) {
        background.color = color;
    }
}
}