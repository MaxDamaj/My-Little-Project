using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupRowWindow : MonoBehaviour {

	public PopupManager PM;
	public SceneManager SM;

	[Header("Row Pile")]
	public GameObject window;
	public Button rowButtonCancel;
	public Button rowButton;
	public Text rowOkText;
	public Text rowText;

	void Start() {
		window.SetActive(false);
		rowButton.onClick.AddListener(PopupRowClick);
		rowButtonCancel.onClick.AddListener(delegate {
			//Return cards state
			for (int i = 0; i < SM.fx_tRow.GetLength(0); i++) {
				if (SM.tRowPile[i].childCount > 0) {
					SM.tRowPile[i].GetChild(0).GetComponent<Draggable>().enabled = true;
					SM.tRowPile[i].GetChild(0).GetComponent<CardInPile>().enabled = false;
				}
			}
			window.SetActive(false);
			SM.RefreshUI();
		});
	}

	public void Refresh() {
		if (window.activeSelf) {
			PM.list = new List<Transform>();
			for (int i = 0; i < SM.tRowPile.GetLength(0); i++) {
				if (SM.tRowPile[i].childCount > 0) {
					if (SM.tRowPile[i].GetChild(0).GetComponent<CardInPile>().IsSelected) {
						PM.list.Add(SM.tRowPile[i].GetChild(0));
						//Remove unnesesary selection
						if (PM.list.Count > PM.actionCount) {
							PM.list[PM.list.Count-1].GetComponent<CardInPile>().IsSelected = false;
							PM.list[PM.list.Count-1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
							PM.list.RemoveAt(PM.list.Count-1);
						}
					}
				}
			}
			if (PM.list.Count == 0) {
				rowOkText.text = "Do not use";
			} else {
				rowOkText.text = "Select "+PM.list.Count;
			}
		}
	}


	//Row popup action
	public void ShowRowPopup(Card card, ActCondition cond) {
		window.SetActive(true);
		PM.callbackCard = card;
		PM.callbackCondition = cond;
		PM.actionCount = PM.callbackCard.ReturnModifier(cond);
		for (int i = 0; i < SM.fx_tRow.GetLength(0); i++) {
			SM.bl_tRow[i].SetActive(false);
			SM.fx_tRow[i].SetActive(true);
			if (SM.tRowPile[i].childCount > 0) {
				SM.tRowPile[i].GetChild(0).GetComponent<Draggable>().enabled = false;
				SM.tRowPile[i].GetChild(0).GetComponent<CardInPile>().enabled = true;
			}
		}
		rowOkText.text = "Disband "+card.ReturnModifier(cond)+" cards from trade row";
		PM.Refresh();
	}
	//OnClick event
	public void PopupRowClick() {
		if (PM.list.Count == PM.actionCount) {
			int quan = PM.list.Count;
			for (int i = 0; i < quan; i++) {
				PM.list[0].GetComponent<Card>().SetActionExecution(ActCondition.Disband, true);
				PM.list[0].SetParent(SM.dPile);
				PM.list.RemoveAt(0);
			}
		}
		//Place card on turn
		if (PM.callbackCard.cardType == CardType.Hero && PM.callbackCondition == ActCondition.Normal) {
			PM.callbackCard.transform.SetParent(SM.nowPlayer.pTurn);
		}
		//Return cards state
		for (int i = 0; i < SM.fx_tRow.GetLength(0); i++) {
			if (SM.tRowPile[i].childCount > 0) {
				SM.tRowPile[i].GetChild(0).GetComponent<Draggable>().enabled = true;
				SM.tRowPile[i].GetChild(0).GetComponent<CardInPile>().enabled = false;
			}
		}
		SM.PlayerBIT += PM.callbackCard.ReturnBIT(PM.callbackCondition);
		SM.PlayerATK += PM.callbackCard.ReturnATK(PM.callbackCondition);
		SM.nowPlayer.PlayerLUV += PM.callbackCard.ReturnLUV(PM.callbackCondition);
		PM.callbackCard.SetActionExecution(PM.callbackCondition, true);
		window.SetActive(false);
		SM.RefreshUI();
	}
	//---------------------------------------------------
	public void SelectCardsForDisband() {
		PM.list = new List<Transform>();
		List<Card> pileList = new List<Card>();

		for (int i = 0; i < SM.tRowPile.GetLength(0); i++) {
			pileList.Add(SM.tRowPile[i].GetChild(0).GetComponent<Card>());
		}
		//Find elements with price 0
		for (int i = 0; i < PM.callbackCard.ReturnModifier(PM.callbackCondition); i++) {
			int highestPrice = 0;
			Card selCard = pileList[0];
			foreach (var card in pileList) {
				if (card.price >= highestPrice && !PM.list.Exists(x => x==card.transform)) {
					highestPrice = card.price;
					selCard = card;
				}
			}
			if (selCard != null) PM.list.Add(selCard.transform);
		}
	}
}
