using System;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.notifications {
public class NotificationHandler : MonoBehaviour {
    public Button closeButton;
    public Button zoomButton;
    public TextMeshProUGUI text;
    public Image icon;

    private Action zoomAction;
    private NotificationsPanelHandler panelHandler;
    
    public void init(string text, string icon, NotificationsPanelHandler panelHandler, EcsEntity target) {
        init(text, icon);
        this.panelHandler = panelHandler;
        zoomButton.onClick.AddListener(() => {
            if (!target.IsNull() && target.IsAlive()) {
                Vector3Int position = target.pos();
                GameView.get().cameraAndMouseHandler.selector.setPosition(position);
                GameView.get().cameraAndMouseHandler.cameraMovementSystem.setCameraTarget(position, false);
            } else {
                panelHandler.closeNotification(this);
            }
        });
        closeButton.onClick.AddListener(() => panelHandler.closeNotification(this));
    }

    public void init(string text, string icon, Vector3Int target) {
        init(text, icon);
        
    }

    private void init(string text, string icon) {
        this.text.text = text;
        this.icon.sprite = IconLoader.get().getSprite("notifications/" + icon);
    }
}
}