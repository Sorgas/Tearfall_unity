using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldmapInputHandler : MonoBehaviour, IMoveHandler, IPointerClickHandler, IPointerDownHandler, IScrollHandler {
    public void OnMove(AxisEventData eventData) {
        Debug.Log("move on worldmap");
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("click on worldmap");
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("down on worldmap");
    }

    public void OnScroll(PointerEventData eventData) {
        Debug.Log(eventData.scrollDelta);
    }
}
