using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.jobs_widget {
public class ButtonRightClickHandler : MonoBehaviour, IPointerClickHandler {
    public List<Action> onRmbClick = new();
    
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            foreach (var action in onRmbClick) {
                action.Invoke();
            }
        }
    }
}
}