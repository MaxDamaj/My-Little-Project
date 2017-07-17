using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckDrop : MonoBehaviour, IDropHandler {

	public Decks targetDeck;
	public SceneManager SM;
    public int CountRestriction;

	public void OnDrop (PointerEventData eventData) {
		Draggable pCard = eventData.pointerDrag.GetComponent<Draggable>();
		if (pCard != null) CardDrop(pCard);

	}

	public void CardDrop(Draggable card) {
		//Menu state
		if (card.beginDeck.targetDeck == Decks.mDeck) {
			if (CountRestriction > 0 && transform.childCount >= CountRestriction) return;
			card.beginDeck = this;
			return;
		}
		//InGame state
		SM.nowPlayer.pileMask.raycastTarget = true;
		if (targetDeck == Decks.pPile) { //Default place
			TradeRowPlacement(card);
		}
		if (targetDeck == Decks.pDeck && SM.onTopOfDesk > 0) { //If on top active
			//Hero on top of deck
			if (card.GetComponent<Card>().cardType == CardType.Hero && SM.onTopType == OnTopType.Hero) {
				TradeRowPlacement(card);
				SM.onTopOfDesk--;
				card.setFirst = true;
			}
			//Warehouses on top of deck
			if (card.GetComponent<Card>().cardType != CardType.Hero && SM.onTopType == OnTopType.Warehouses) {
				TradeRowPlacement(card);
				SM.onTopOfDesk--;
				card.setFirst = true;
			}
			//Card on top of deck
			if (SM.onTopType == OnTopType.All) {
				TradeRowPlacement(card);
				SM.onTopOfDesk--;
				card.setFirst = true;
			}

		}
		//Hand state
		if (card.beginDeck.targetDeck == Decks.pHand) {
			if (card.GetComponent<Card>().cardType == CardType.Hero && targetDeck == Decks.pTurn) {
				card.beginDeck = this;
			}
			if ((card.GetComponent<Card>().cardType == CardType.Warehouse || card.GetComponent<Card>().cardType == CardType.Castle || card.GetComponent<Card>().cardType == CardType.Village) && targetDeck == Decks.pWarehouses) {
				card.transform.Rotate(Vector3.forward, -90);
				card.beginDeck = this;
			}
		}
	}

	void TradeRowPlacement(Draggable card) {
		switch (card.beginDeck.targetDeck) {
		case Decks.tProsp:
			GameObject tmp = Instantiate(SM.U04);
			tmp.transform.SetParent(card.beginDeck.transform);
			tmp.transform.localScale = new Vector3(1, 1, 1);
			SM.PlayerBIT -= card.GetComponent<Card>().price;
			SM.RefreshUI();
			card.beginDeck = this;
			break;
		case Decks.tLow:
			SM.PlayerBIT -= card.GetComponent<Card>().price;
			SM.RefreshUI();
			card.beginDeck = this;
			break;
		case Decks.tMiddle:
			SM.PlayerBIT -= card.GetComponent<Card>().price;
			SM.RefreshUI();
			card.beginDeck = this;
			break;
		}
	}

}
