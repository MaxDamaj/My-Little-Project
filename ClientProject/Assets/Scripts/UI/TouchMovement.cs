using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using MLA.System;
using MLA.Gameplay.Controllers;

namespace MLA.Gameplay.Common {
    public class TouchMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        public GameObject MoveBack;
        public GameObject MoveUp;
        public GameObject MoveForward;

        public void OnPointerDown(PointerEventData eventData) {
            if (GlobalData.Instance.gameState == GameModeState.Endurance) {
                if (eventData.pointerEnter == MoveBack) {
                    PonyController.Instance.SetShift(1);
                }
                if (eventData.pointerEnter == MoveUp) {
                    PonyController.Instance.SetShift(-1);
                }
            }
            if (GlobalData.Instance.gameState == GameModeState.Challenge) {
                if (eventData.pointerEnter == MoveBack) {
                    PonyFreeMoveController.Instance.SetSideShift(1);
                }
                if (eventData.pointerEnter == MoveUp) {
                    PonyFreeMoveController.Instance.SetSideShift(-1);
                }
                if (eventData.pointerEnter == MoveForward) {
                    PonyFreeMoveController.Instance.SetFrontShift(1);
                }
            }

        }
        public void OnPointerUp(PointerEventData eventData) {
            if (GlobalData.Instance.gameState == GameModeState.Endurance) {
                if (eventData.pointerEnter == MoveBack) {
                    PonyController.Instance.SetShift(0);
                }
                if (eventData.pointerEnter == MoveUp) {
                    PonyController.Instance.SetShift(0);
                }
            }
            if (GlobalData.Instance.gameState == GameModeState.Challenge) {
                if (eventData.pointerEnter == MoveBack) {
                    PonyFreeMoveController.Instance.SetSideShift(0);
                }
                if (eventData.pointerEnter == MoveUp) {
                    PonyFreeMoveController.Instance.SetSideShift(0);
                }
                if (eventData.pointerEnter == MoveForward) {
                    PonyFreeMoveController.Instance.SetFrontShift(0);
                }
            }
        }

    }
}
