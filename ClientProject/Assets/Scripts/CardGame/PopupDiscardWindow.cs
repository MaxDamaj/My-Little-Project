using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupDiscardWindow : MonoBehaviour {

	public PopupManager PM;
	public SceneManager SM;

	[Header("Discard Popup")]
	public GameObject window;
	public RectTransform disHand;
	public Text discardText;
	public Button discardButton;
	public Text discardButText;

	void Start() {
		window.SetActive(false);
		discardButton.onClick.AddListener(PopupDiscardClick);
	}

	public void Refresh() {
		if (window.activeSelf) {
			PM.list = new List<Transform>();
			for (int i = 0; i < disHand.childCount; i++) {
				if (disHand.GetChild(i).GetComponent<CardInPile>().IsSelected) {
					PM.list.Add(disHand.GetChild(i));
					//Remove unnesesary selection
					if (PM.list.Count > SM.discardCount) {
						PM.list[PM.list.Count-1].GetComponent<CardInPile>().IsSelected = false;
						PM.list[PM.list.Count-1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
						PM.list.RemoveAt(PM.list.Count-1);
					}
				}
			}
			//Set Button info
			if (PM.list.Count == 0) {
				discardButText.text = "Select "+SM.discardCount+" cards";
			} else {
				discardButText.text = "Select " + PM.list.Count;
			}
		}
	}



	//Discard popup action
	public void ShowDiscardPopup() {
		window.SetActive(true);
		int quan = SM.nowPlayer.pHand.childCount;
		for (int i = 0; i < quan; i++) {
			SM.nowPlayer.pHand.GetChild(0).GetComponent<Draggable>().enabled = false;
			SM.nowPlayer.pHand.GetChild(0).GetComponent<CardInPile>().enabled = true;
			SM.nowPlayer.pHand.GetChild(0).SetParent(disHand);
		}
		discardText.text = "Discard "+SM.discardCount+" cards";
		PM.Refresh();
	}
	//OnClick event
	public void PopupDiscardClick() {
		if (PM.list.Count == SM.discardCount) {
			int quan = PM.list.Count;
			for (int i = 0; i < quan; i++) {
				PM.list[0].GetComponent<Draggable>().enabled = true;
				PM.list[0].GetComponent<CardInPile>().enabled = false;
				PM.list[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
				PM.list[0].SetParent(SM.nowPlayer.pPile);
				PM.list.RemoveAt(0);
			}
			quan = disHand.childCount;
			for (int i = 0; i < quan; i++) {
				disHand.GetChild(0).GetComponent<Draggable>().enabled = true;
				disHand.GetChild(0).GetComponent<CardInPile>().enabled = false;
				disHand.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
				disHand.GetChild(0).SetParent(SM.nowPlayer.pHand);
			}
			SM.discardCount = 0;
			window.SetActive(false);
		}
	}
	//---------------------------------------------------
	public void SelectCardsWithLowestPrice() {
		PM.list = new List<Transform>();
		List<Card> handList = new List<Card>();

		for (int i = 0; i < disHand.childCount; i++) {
			handList.Add(disHand.GetChild(i).GetComponent<Card>());
		}
		//Find elements with highest price
		for (int i = 0; i < SM.discardCount; i++) {
			int lowestPrice = 99;
			Card selCard = handList[0];
			foreach (var card in handList) {
				if (card.price <= lowestPrice && !PM.list.Exists(x => x==card.transform)) {
					lowestPrice = card.price;
					selCard = card;
				}
			}
			PM.list.Add(selCard.transform);
		}
	}

}
