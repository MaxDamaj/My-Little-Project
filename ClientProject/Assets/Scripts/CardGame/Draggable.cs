using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {

	public DeckDrop beginDeck;
	public bool setFirst = false;

	private Transform Table;
	private Card card;

	void Start() {
		Table = GameObject.Find("UI").transform;
		card = GetComponent<Card>();
	}

	//------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData) {
		beginDeck = transform.parent.GetComponent<DeckDrop>();
		if (beginDeck == null) return;
		//Large popup
		if (eventData.button == PointerEventData.InputButton.Right) {
			if (beginDeck.SM != null) beginDeck.SM.PM.ShowLargePopup(GetComponent<Image>().sprite, card);
		}
		//Card start dragging
		if (eventData.button == PointerEventData.InputButton.Left) {
			CardActions();
		}
	}

	public void OnBeginDrag(PointerEventData eventData) {
		CardStartDrag();
	}

	public void OnDrag(PointerEventData eventData) {
		if (!MovementCheck(beginDeck)) {return;}
		//Moving card with pointer
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData) {
		//In Menu state
		if (beginDeck.targetDeck == Decks.mDeck) {
			transform.SetParent(beginDeck.transform);
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			beginDeck = transform.parent.GetComponent<DeckDrop>();
			return;
		}
		//Attach card
		CardEndDrag();
	}

	//------------------------------------------------------------------------------------
	//Check movement ability from current deck
	bool MovementCheck(DeckDrop deck) {
		if (deck.targetDeck == Decks.pPile) {return false;}
		if (deck.targetDeck == Decks.pTurn) {return false;}
		if (deck.targetDeck == Decks.pDeck) {return false;}
		if (deck.targetDeck == Decks.pWarehouses) {return false;}
		if (deck.targetDeck == Decks.pHand && card.GetActionVariation(ActCondition.Normal) == Variation.OR) {return false;}
		if (deck.targetDeck == Decks.pHand && card.ReturnInAddition(ActCondition.Normal) == SpecAction.DisbandRow) {return false;}
		return true;
	}

	//Actions for card if its start dragging
	public void CardActions() {
		beginDeck = transform.parent.GetComponent<DeckDrop>();
		//Disband AND, OR state (Hero)
		if (beginDeck.targetDeck == Decks.pTurn && !card.GetActionExecution(ActCondition.Disband)) {
			if (card.GetActionVariation(ActCondition.Disband) == Variation.AND) {
				beginDeck.SM.PM.popupANDWindow.ShowANDPopup(card, ActCondition.Disband);
			} else {
				beginDeck.SM.PM.popupORWindow.ShowORPopup(card, ActCondition.Disband);
			}
		}
		//Disband AND, OR state (Warehouse)
		if (beginDeck.targetDeck == Decks.pWarehouses && !card.GetActionExecution(ActCondition.Disband) &&
			card.GetActionExecution(ActCondition.Normal) && card.GetActionExecution(ActCondition.Tribe)) {
			if (card.GetActionVariation(ActCondition.Disband) == Variation.AND) {
				beginDeck.SM.PM.popupANDWindow.ShowANDPopup(card, ActCondition.Disband);
			} else {
				beginDeck.SM.PM.popupORWindow.ShowORPopup(card, ActCondition.Disband);
			}
		}
		if (beginDeck.targetDeck == Decks.pWarehouses && !card.GetActionExecution(ActCondition.Normal)) {
			//Normal OR state for warehouses
			if (card.GetActionVariation(ActCondition.Normal) == Variation.OR) {
				beginDeck.SM.PM.popupORWindow.ShowORPopup(card, ActCondition.Normal);
			}
			if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.Disband_HandAndDiscard) {
				beginDeck.SM.PM.popupPileWindow.ShowPilePopup(card, ActCondition.Normal);
			}
		}

		if (beginDeck.targetDeck == Decks.pHand) {
			//Normal OR state
			if (card.GetActionVariation(ActCondition.Normal) == Variation.OR) {
				beginDeck.SM.PM.popupORWindow.ShowORPopup(card, ActCondition.Normal);
			}
			//Normal Disband H&P
			if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.Disband_HandAndDiscard) {
				beginDeck.SM.PM.popupPileWindow.ShowPilePopup(card, ActCondition.Normal);
			}
			//Normal Disband Row
			if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.DisbandRow) {
				beginDeck.SM.PM.popupRowWindow.ShowRowPopup(card, ActCondition.Normal);
			}
		}
		if ((beginDeck.targetDeck == Decks.pTurn || beginDeck.targetDeck == Decks.pWarehouses) && !card.GetActionExecution(ActCondition.Tribe) &&
			beginDeck.SM.CheckTribe(card.tribe) && card.GetActionExecution(ActCondition.Normal)) {
			//Tribe OR state
			if (card.GetActionVariation(ActCondition.Tribe) == Variation.OR) {
				beginDeck.SM.PM.popupORWindow.ShowORPopup(card, ActCondition.Tribe);
			}
			//Tribe Disband H&P
			if (card.ReturnInAddition(ActCondition.Tribe) == SpecAction.Disband_HandAndDiscard) {
				beginDeck.SM.PM.popupPileWindow.ShowPilePopup(card, ActCondition.Tribe);
			}
			//Normal Disband Row
			if (card.ReturnInAddition(ActCondition.Tribe) == SpecAction.DisbandRow) {
				beginDeck.SM.PM.popupRowWindow.ShowRowPopup(card, ActCondition.Tribe);
			}
		}
	}

	public void CardStartDrag() {
		if (Table == null) Table = GameObject.Find("UI").transform;
		beginDeck = transform.parent.GetComponent<DeckDrop>();
		//In Menu state
		if (beginDeck.targetDeck == Decks.mDeck) {
			GetComponent<CanvasGroup>().blocksRaycasts = false;
			transform.SetParent(Table);
			return;
		}

		if (!MovementCheck(beginDeck)) {return;}
		beginDeck.SM.SetFX(beginDeck, card);
		beginDeck.SM.nowPlayer.pileMask.raycastTarget = false;
		//Detach card
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		transform.SetParent(Table);
	}

	public void CardEndDrag() {
		beginDeck.SM.fx.gameObject.SetActive(false);
		transform.SetParent(beginDeck.transform);
		if (setFirst) {
			transform.SetAsFirstSibling();
			setFirst = false;
		}
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		beginDeck = transform.parent.GetComponent<DeckDrop>();
		if (card.ReturnInAddition(ActCondition.Normal) == SpecAction.DisbandRow) return;
		beginDeck.SM.RefreshUI();
	}

}
