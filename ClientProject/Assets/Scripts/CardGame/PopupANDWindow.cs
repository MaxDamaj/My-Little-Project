using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupANDWindow : MonoBehaviour {
	
	public PopupManager PM;
	public SceneManager SM;

	[Header("AND Popup")]
	public GameObject window;
	public GameObject ANDBitImage;
	public Text ANDBitText;
	public GameObject ANDAtkImage;
	public Text ANDAtkText;
	public GameObject ANDLuvImage;
	public Text ANDLuvText;
	public Text ANDSpecText;

	void Start() {
		window.SetActive(false);
	}

	public void Refresh() {

	}


	//AND popup action
	public void ShowANDPopup(Card card, ActCondition cond) {
		window.SetActive(true);
		PM.screenBlocker.SetActive(true);
		PM.callbackCard = card;
		PM.callbackCondition = cond;
		if (card.ReturnBIT(cond) > 0) {
			ANDBitText.text = "" + card.ReturnBIT(cond);
			ANDBitImage.SetActive(true);
		} else {
			ANDBitImage.SetActive(false);
		}
		if (card.ReturnATK(cond) > 0) {
			ANDAtkText.text = "" + card.ReturnATK(cond);
			ANDAtkImage.SetActive(true);
		} else {
			ANDAtkImage.SetActive(false);
		}
		if (card.ReturnLUV(cond) > 0) {
			ANDLuvText.text = "" + card.ReturnLUV(cond);
			ANDLuvImage.SetActive(true);
		} else {
			ANDLuvImage.SetActive(false);
		}
		if (card.ReturnInAddition(cond) != SpecAction.None) {
			ANDSpecText.gameObject.SetActive(true);
			switch (card.ReturnInAddition(cond)) {
			case SpecAction.DrawACard:
				ANDSpecText.text = "Draw "+card.ReturnModifier(cond)+" card";
				break;
			case SpecAction.Opp_DiscardCard:
				ANDSpecText.text = "Target opponent discard "+card.ReturnModifier(cond)+" card";
				break;
			}
		} else {
			ANDSpecText.gameObject.SetActive(false);
		}
		window.GetComponent<Button>().onClick.RemoveAllListeners();
		window.GetComponent<Button>().onClick.AddListener(PopupANDClick);
	}

	//OnClick event
	void PopupANDClick() {
		window.SetActive(false);
		PM.screenBlocker.SetActive(false);
		//-------
		PM.callbackCard.SetActionExecution(PM.callbackCondition, true);
		SM.PlayerBIT += PM.callbackCard.ReturnBIT(PM.callbackCondition);
		SM.PlayerATK += PM.callbackCard.ReturnATK(PM.callbackCondition);
		SM.nowPlayer.PlayerLUV += PM.callbackCard.ReturnLUV(PM.callbackCondition);
		if (PM.callbackCard.ReturnInAddition(PM.callbackCondition) == SpecAction.DrawACard) {
			SM.PutCardsOnPile(PM.callbackCard.ReturnModifier(ActCondition.Disband), SM.nowPlayer.pDeck, SM.nowPlayer.pHand, SM.nowPlayer.pPile, true);
		}
		if (PM.callbackCard.ReturnInAddition(PM.callbackCondition) == SpecAction.Opp_DiscardCard) {
			SM.discardCount += PM.callbackCard.ReturnModifier(ActCondition.Disband);
		}
		if (PM.callbackCondition == ActCondition.Disband) {
			PM.callbackCard.transform.SetParent(SM.dPile);
		}
		SM.RefreshUI();
	}

}
