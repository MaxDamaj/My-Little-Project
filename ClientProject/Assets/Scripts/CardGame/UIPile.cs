using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIPile : MonoBehaviour, IPointerClickHandler {

	public SceneManager SM;
    public Sprite UIMask;

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (SM.nowPlayer.pPile.childCount != 0) {
				SM.PM.popupPileWindow.ShowPilePopup(SM.nowPlayer.pPile);
			}
		}
	}

}
