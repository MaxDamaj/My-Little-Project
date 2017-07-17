using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupORWindow : MonoBehaviour {

	public PopupManager PM;
	public SceneManager SM;

	[Header("OR Popup")]
	public GameObject window;
	public Button ORBitButton;
	public Button ORAtkButton;
	public Button ORLuvButton;
	public Button ORActionButton;
	public Text ORBitText;
	public Text ORAtkText;
	public Text ORLuvText;
	public Text ORActionText;

	void Start() {
		window.SetActive(false);
	}

	public void Refresh() {
	
	}


	//OR popup action
	public void ShowORPopup(Card card, ActCondition cond) {
		PM.callbackCard = card;
		PM.callbackCondition = cond;
		window.SetActive(true);
		PM.screenBlocker.SetActive(true);
		ORActionButton.gameObject.SetActive(false);

		if (card.ReturnBIT(cond) > 0) {
			ORBitButton.gameObject.SetActive(true);
			ORBitText.text = ""+card.ReturnBIT(cond);
		} else {
			ORBitButton.gameObject.SetActive(false);
		}
		if (card.ReturnATK(cond) > 0) {
			ORAtkButton.gameObject.SetActive(true);
			ORAtkText.text = ""+card.ReturnATK(cond);
		} else {
			ORAtkButton.gameObject.SetActive(false);
		}
		if (card.ReturnLUV(cond) > 0) {
			ORLuvButton.gameObject.SetActive(true);
			ORLuvText.text = ""+card.ReturnLUV(cond);
		} else {
			ORLuvButton.gameObject.SetActive(false);
		}

	}
	//OnClick event
	public void PopupORClick(string statement) {
		if (PM.callbackCondition == ActCondition.Normal) {
			if (PM.callbackCard.cardType == CardType.Hero) {
				PM.callbackCard.transform.SetParent(SM.nowPlayer.pTurn);
			} else {
				PM.callbackCard.transform.SetParent(SM.nowPlayer.pWarehouses);
				PM.callbackCard.transform.rotation = Quaternion.identity;
				PM.callbackCard.transform.Rotate(Vector3.forward, -90);
			}
		}
		if (statement == "BIT") {
			SM.PlayerBIT += PM.callbackCard.ReturnBIT(PM.callbackCondition);
		}
		if (statement == "ATK") {
			SM.PlayerATK += PM.callbackCard.ReturnATK(PM.callbackCondition);
		}
		if (statement == "LUV") {
			SM.nowPlayer.PlayerLUV += PM.callbackCard.ReturnLUV(PM.callbackCondition);
		}
		//Execute Special actions (not in OR state)
		if (PM.callbackCard.ReturnInAddition(PM.callbackCondition) == SpecAction.DrawACard) {
			SM.PutCardsOnPile(1, SM.nowPlayer.pDeck, SM.nowPlayer.pHand, SM.nowPlayer.pPile, true);
		}
		if (PM.callbackCard.ReturnInAddition(PM.callbackCondition) == SpecAction.Opp_DiscardCard) {
			SM.discardCount += PM.callbackCard.ReturnModifier(PM.callbackCondition);
		}
		//If Disband action put in disband pile
		if (PM.callbackCondition == ActCondition.Disband) {
			PM.callbackCard.transform.SetParent(SM.dPile);
		}

		PM.callbackCard.SetActionExecution(PM.callbackCondition, true);
		window.SetActive(false);
		PM.screenBlocker.SetActive(false);
		SM.RefreshUI();
	}
	//---------------------------------------------------
	public string ReturnFirstActiveState(string preferState) {
		switch (preferState) {
		case "BIT":
			if (ORBitButton.gameObject.activeSelf) return "BIT";
			break;
		case "ATK":
			if (ORAtkButton.gameObject.activeSelf) return "ATK";
			break;
		case "LUV":
			if (ORLuvButton.gameObject.activeSelf) return "LUV";
			break;
		}
		if (ORBitButton.gameObject.activeSelf) return "BIT";
		if (ORAtkButton.gameObject.activeSelf) return "ATK";
		if (ORLuvButton.gameObject.activeSelf) return "LUV";
		return "";
	}
}
