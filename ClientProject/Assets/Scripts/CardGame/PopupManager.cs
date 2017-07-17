using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour {

	public SceneManager SM;

	[Header("Popups")]
	public PopupANDWindow popupANDWindow;
	public PopupORWindow popupORWindow;
	public GameObject PopupLarge;
	public PopupPileWindow popupPileWindow;
	public PopupWinnerWindow popupWinnerWindow;
	public PopupDiscardWindow popupDiscardWindow;
	public PopupRowWindow popupRowWindow;

	[Header("Large Popup")]
	public Button largeButton;
	public Image largeImage;

	[Header("Menu Popup")]
	public Button menuButton;

	[Header("Common")]
	public GameObject screenBlocker;
	public int actionCount;
	public Card callbackCard;
	public ActCondition callbackCondition;
	public List<Transform> list;

	// Use this for initialization
	void Start() {
		screenBlocker.SetActive(false);
		//Disable popups
		PopupLarge.SetActive(false);
		//Button listeners
		largeButton.onClick.AddListener(delegate {
			if (callbackCard.cardType != CardType.Hero) largeImage.transform.Rotate(Vector3.forward, 90);
			PopupLarge.SetActive(false);
		});


		menuButton.onClick.AddListener(delegate {
			UnityEngine.SceneManagement.SceneManager.LoadScene("menu");	
		});
	}

	public void Refresh() {
		//Check Winning
		if (SM.enemy1.LUV <= 0) {
			SM.enemy1.LUV = 0;
			SM.enemy1.Refresh();
			popupWinnerWindow.ShowPopup(true);
		}
		if (SM.enemy2.LUV <= 0) {
			SM.enemy2.LUV = 0;
			SM.enemy2.Refresh();
			popupWinnerWindow.ShowPopup(false);
		}
		popupPileWindow.Refresh();
		popupDiscardWindow.Refresh();
		popupRowWindow.Refresh();
	}

	//Large popup action
	public void ShowLargePopup(Sprite image, Card card) {
		callbackCard = card;
		if (card.cardType != CardType.Hero) {
			largeImage.transform.Rotate(Vector3.forward, -90);
		}
		largeImage.sprite = image;
		PopupLarge.SetActive(true);
	}

	//------------------------------------------------------------
	public PopupState ReturnActivePopup() {
		if (popupORWindow.window.activeSelf) return PopupState.OR;
		if (popupANDWindow.window.activeSelf) return PopupState.AND;
		if (popupPileWindow.window.activeSelf) return PopupState.DisbandPile;
		if (popupDiscardWindow.window.activeSelf) return PopupState.Discard;
		if (popupRowWindow.window.activeSelf) return PopupState.DisbandRow;
		return PopupState.Normal;
	}

}
