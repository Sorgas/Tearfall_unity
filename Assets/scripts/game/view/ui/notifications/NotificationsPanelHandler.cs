using System.Collections.Generic;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.notifications {
public class NotificationsPanelHandler : MonoBehaviour {
    public RectTransform notificationsPanel;
    private List<NotificationHandler> notifications = new();
    
    public void Update() {
        
    }

    public void addNotification(string text, string icon, EcsEntity entity) {
        GameObject notification = PrefabLoader.create("Notification", notificationsPanel, new Vector3(0, 45 * notifications.Count, 0));
        notification.GetComponent<NotificationHandler>().init(text, icon, this, entity);
        notifications.Add(notification.GetComponent<NotificationHandler>());
    }

    public void closeNotification(NotificationHandler notification) {
        notifications.Remove(notification);
        Destroy(notification.gameObject);
    }
}
}