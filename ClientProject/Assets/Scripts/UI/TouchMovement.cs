using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public GameObject MoveBack;
    public GameObject MoveUp;


    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.pointerEnter == MoveBack) {
            PonyController.Instance.SetShift(1);
        }
        if (eventData.pointerEnter == MoveUp) {
            PonyController.Instance.SetShift(-1);
        }
    }
    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.pointerEnter == MoveBack) {
            PonyController.Instance.SetShift(0);
        }
        if (eventData.pointerEnter == MoveUp) {
            PonyController.Instance.SetShift(0);
        }
    }



}
