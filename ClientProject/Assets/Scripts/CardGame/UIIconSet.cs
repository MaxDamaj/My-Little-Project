using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIIconSet : MonoBehaviour, IPointerDownHandler {

	public MenuManager MM;
	public GameObject iconsWindow;

	public void OnPointerDown(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			MM.nowPlayerIcon.sprite = GetComponent<Image>().sprite;
			iconsWindow.SetActive(false);
		}
	}
}
