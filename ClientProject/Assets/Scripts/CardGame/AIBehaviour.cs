using UnityEngine;
using System.Collections;

public class AIBehaviour : MonoBehaviour {

	public SceneManager SM;

	private IEnumerator checkingSteps;
	private string state;
	private int turnNumber;

	void Start() {
		turnNumber = 0;
	}
	void OnEnable() {
		checkingSteps = CheckSteps();
		StartCoroutine(checkingSteps);
	}	
	void OnDisable() {
		StopAllCoroutines();
	}


	IEnumerator CheckSteps() {
		Draggable card = null;
		yield return new WaitForSeconds(0.5f);
		while (true) {
			state = GetState();
			Debug.Log("state = "+state);
			switch (state) {
			case "PopupAction":
				PopupState popupState = SM.PM.ReturnActivePopup();
				//OR popup
				if (popupState == PopupState.OR) {
					if (SM.nowPlayer.PlayerLUV <= 20) {
						SM.PM.popupORWindow.PopupORClick(SM.PM.popupORWindow.ReturnFirstActiveState("LUV"));
					} else {
						if (turnNumber < 5) {
							SM.PM.popupORWindow.PopupORClick(SM.PM.popupORWindow.ReturnFirstActiveState("BIT"));
						} else {
							SM.PM.popupORWindow.PopupORClick(SM.PM.popupORWindow.ReturnFirstActiveState("ATK"));
						}
					}
				}
				//Discard popup
				if (popupState == PopupState.Discard) {
					SM.PM.popupDiscardWindow.SelectCardsWithLowestPrice();
					SM.PM.popupDiscardWindow.PopupDiscardClick();
				}
				//Disband popup
				if (popupState == PopupState.DisbandPile) {
					SM.PM.popupPileWindow.SelectCardsToDisband();
					SM.PM.popupPileWindow.PopupPileExecute();
				}
				//Disband Row popup
				if (popupState == PopupState.DisbandRow) {
					SM.PM.popupRowWindow.SelectCardsForDisband();
					SM.PM.popupRowWindow.PopupRowClick();
				}
				break;
			case "PlayCards":
				card = SM.nowPlayer.pHand.GetChild(0).GetComponent<Draggable>();
				card.CardActions();
				if (SM.PM.ReturnActivePopup() == PopupState.Normal) {
					card.CardStartDrag();
					if (!SM.PM.screenBlocker.activeSelf) {
						if (card.GetComponent<Card>().cardType == CardType.Hero) {
							SM.nowPlayer.pTurn.GetComponent<DeckDrop>().CardDrop(card);
						} else {
							SM.nowPlayer.pWarehouses.GetComponent<DeckDrop>().CardDrop(card);
						}
						card.CardEndDrag();
					}
				}
				break;
			case "PlayWarehouses":
				card = SM.ReturnUnusedExtraWarehouse();
				card.CardActions();
				break;
			case "Buying":
				for (int i = SM.tRowPile.GetLength(0) - 1; i > 0; i--) {
					if (SM.tRowPile[i].childCount > 0) {
						if (SM.tRowPile[i].GetChild(0).GetComponent<Card>().price <= SM.PlayerBIT) {
							card = SM.tRowPile[i].GetChild(0).GetComponent<Draggable>();
							card.CardActions();
							card.CardStartDrag();
							SM.nowPlayer.pPile.GetComponent<DeckDrop>().CardDrop(card);
							card.CardEndDrag();
							yield return new WaitForSeconds(0.5f);
						}		
					}
				}
				break;
			case "Attack":
				while (SM.nowEnemy.ReturnDestroyableWarehouse(SM.PlayerATK) != null) {
					var target = SM.nowEnemy.ReturnDestroyableWarehouse(SM.PlayerATK);
					SM.nowEnemy.AttackWarehouse(target);
					yield return new WaitForSeconds(0.5f);
				}
				SM.nowEnemy.AttackEnemy();
				yield return new WaitForSeconds(0.7f);
				turnNumber++;
				SM.EndTurnClick();
				break;
			case "EndTurn":
				turnNumber++;
				SM.EndTurnClick();
				break;
			}
			yield return new WaitForSeconds(0.7f);
		}
	}

	string GetState() {
		if (SM.PM.ReturnActivePopup() != PopupState.Normal) {
			return "PopupAction";
		}
		if (SM.nowPlayer.pHand.childCount > 0)
			return "PlayCards";
		if (SM.ReturnUnusedExtraWarehouse() != null) {
			return "PlayWarehouses";
		}
		for (int i = SM.tRowPile.GetLength(0) - 1; i > 0; i--) {
			if (SM.tRowPile[i].childCount > 0) {
				if (SM.tRowPile[i].GetChild(0).GetComponent<Card>().price <= SM.PlayerBIT)
					return "Buying";
			}
		}
		if (SM.PlayerATK > 0)
			return "Attack";
		return "EndTurn";
	}
}
