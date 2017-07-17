using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupPileWindow : MonoBehaviour {

	public PopupManager PM;
	public SceneManager SM;

	[Header("Pile Popup")]
	public GameObject window;
	public RectTransform cPile;
	public RectTransform cHand;
	public GameObject cMessage;
	public Button pileButton;
	public Button pileCancel;
	public Button pileOk;
	public Text pileOkText;
	public Text pileText;

	void Start() {
		window.SetActive(false);
		pileButton.onClick.AddListener(PopupPileClick);
		pileCancel.onClick.AddListener(PopupPileClick);
		pileOk.onClick.AddListener(PopupPileExecute);
	}

	public void Refresh() {
		if (window.activeSelf) {
			PM.list = new List<Transform>();
			for (int i = 0; i < cHand.childCount; i++) {
				if (cHand.GetChild(i).GetComponent<CardInPile>().IsSelected) {
					PM.list.Add(cHand.GetChild(i));
					//Remove unnesesary selection
					if (PM.list.Count > PM.actionCount || PM.list.Exists(x => x==PM.callbackCard.transform)) {
						PM.list[PM.list.Count-1].GetComponent<CardInPile>().IsSelected = false;
						PM.list[PM.list.Count-1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
						PM.list.RemoveAt(PM.list.Count-1);
					}
				}
			}
			for (int i = 0; i < cPile.childCount; i++) {
				if (cPile.GetChild(i).GetComponent<CardInPile>().IsSelected) {
					PM.list.Add(cPile.GetChild(i));
					//Remove unnesesary selection
					if (PM.list.Count > PM.actionCount) {
						PM.list[PM.list.Count-1].GetComponent<CardInPile>().IsSelected = false;
						PM.list[PM.list.Count-1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
						PM.list.RemoveAt(PM.list.Count-1);
					}
				}
			}
			//Set Button info
			if (PM.list.Count == 0) {
				pileOkText.text = "Do Not Use";
			} else {
				pileOkText.text = "Select " + PM.list.Count;
			}
		}
	}


	//Pile popup action
	public void ShowPilePopup(Transform begin) {
		window.SetActive(true);
		cMessage.SetActive(false);
		int quan = begin.childCount;
		for (int i = 0; i < quan; i++) {
			begin.GetChild(0).GetComponent<Draggable>().enabled = false;
			begin.GetChild(0).SetParent(cPile);
		}
	}
	public void ShowPilePopup(Card card, ActCondition cond) {
		PM.callbackCard = card;
		PM.callbackCondition = cond;
		PM.actionCount = card.ReturnModifier(cond);
		window.SetActive(true);
		cMessage.SetActive(true);
		int quan = SM.nowPlayer.pHand.childCount;
		for (int i = 0; i < quan; i++) {
			SM.nowPlayer.pHand.GetChild(0).GetComponent<Draggable>().enabled = false;
			SM.nowPlayer.pHand.GetChild(0).GetComponent<CardInPile>().enabled = true;
			SM.nowPlayer.pHand.GetChild(0).SetParent(cHand);
		}
		quan = SM.nowPlayer.pPile.childCount;
		for (int i = 0; i < quan; i++) {
			SM.nowPlayer.pPile.GetChild(0).GetComponent<Draggable>().enabled = false;
			SM.nowPlayer.pPile.GetChild(0).GetComponent<CardInPile>().enabled = true;
			SM.nowPlayer.pPile.GetChild(0).SetParent(cPile);
		}
		//Execute specAction
		if (card.ReturnInAddition(cond) == SpecAction.Disband_HandAndDiscard) {
			pileText.text = "Disband "+card.ReturnModifier(cond)+" cards from your hand or discard pile";
		}
		PM.Refresh();
	}
	//OnClick event
	public void PopupPileExecute() {
		if (PM.callbackCondition == ActCondition.Normal) {
			if (PM.callbackCard.cardType == CardType.Hero) {
				PM.callbackCard.transform.SetParent(SM.nowPlayer.pTurn);
			} else {
				PM.callbackCard.transform.SetParent(SM.nowPlayer.pWarehouses);
				PM.callbackCard.transform.rotation = Quaternion.identity;
				PM.callbackCard.transform.Rotate(Vector3.forward, -90);
			}
			PM.callbackCard.GetComponent<Draggable>().enabled = true;
			PM.callbackCard.GetComponent<CardInPile>().enabled = false;
		}
		SM.PlayerBIT += PM.callbackCard.ReturnBIT(PM.callbackCondition);
		SM.PlayerATK += PM.callbackCard.ReturnATK(PM.callbackCondition);
		SM.nowPlayer.PlayerLUV += PM.callbackCard.ReturnLUV(PM.callbackCondition);
		PM.callbackCard.SetActionExecution(PM.callbackCondition, true);
		if (PM.callbackCard.ReturnInAddition(PM.callbackCondition) == SpecAction.Disband_HandAndDiscard) {
			int quan = PM.list.Count;
			for (int i = 0; i < quan; i++) {
				PM.list[0].GetComponent<Card>().SetActionExecution(ActCondition.Disband, true);
				PM.list[0].SetParent(SM.dPile);
				PM.list.RemoveAt(0);
			}
		}
		PopupPileClick();
	}

	void PopupPileClick() {
		int quan = cPile.childCount;
		for (int i = 0; i < quan; i++) {
			cPile.GetChild(0).GetComponent<Draggable>().enabled = true;
			cPile.GetChild(0).GetComponent<CardInPile>().enabled = false;
			cPile.GetChild(0).SetParent(SM.nowPlayer.pPile);
		}
		quan = cHand.childCount;
		for (int i = 0; i < quan; i++) {
			cHand.GetChild(0).GetComponent<Draggable>().enabled = true;
			cHand.GetChild(0).GetComponent<CardInPile>().enabled = false;
			cHand.GetChild(0).SetParent(SM.nowPlayer.pHand);
		}
		window.SetActive(false);
		SM.RefreshUI();
	}
	//---------------------------------------------------
	public void SelectCardsToDisband() {
		PM.list = new List<Transform>();
		List<Card> pileList = new List<Card>();

		for (int i = 0; i < cPile.childCount; i++) {
			pileList.Add(cPile.GetChild(i).GetComponent<Card>());
		}
		//Find elements with price 0
		for (int i = 0; i < PM.callbackCard.ReturnModifier(PM.callbackCondition); i++) {
			int lowestPrice = 0;
			Card selCard = null;
			foreach (var card in pileList) {
				if (card.price <= lowestPrice && !PM.list.Exists(x => x==card.transform)) {
					selCard = card;
				}
			}
			if (selCard != null) PM.list.Add(selCard.transform);
		}
	}

}
