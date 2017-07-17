using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CardInPile : MonoBehaviour, IPointerDownHandler {

	public Color selectColor;
	public bool IsSelected;
	private PopupManager PM;

	void Start() {
		PM = GameObject.Find("PopupManager").GetComponent<PopupManager>();
	}

	void OnDisable() {
		IsSelected = false;
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (IsSelected) {
				GetComponent<Image>().color = new Color(1, 1, 1, 1);
				IsSelected = false;
			} else {
				GetComponent<Image>().color = selectColor;
				IsSelected = true;
			}
			PM.Refresh();
		}
	}

}
