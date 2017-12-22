using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MLADecks {
    Storage, Furnace, Result, Deck, Belt
};

namespace MLA.UI.Common {
    public class MLADeckDrop : MonoBehaviour, IDropHandler {

        public MLADecks targetDeck;
        public int CountRestriction;

        public void OnDrop(PointerEventData eventData) {

            MLADraggable card = eventData.pointerDrag.GetComponent<MLADraggable>();
            CraftComponent item = eventData.pointerDrag.GetComponent<CraftComponent>();

            if (transform.childCount >= CountRestriction && CountRestriction != 0) return;

            if (card != null && item != null) {

                if (card.beginDeck == null) return;
                if (!item.IsItem) {
                    //Material restrictions
                    if (card.beginDeck.targetDeck == MLADecks.Storage && targetDeck == MLADecks.Furnace) {
                        GameObject tmp = Instantiate(card.gameObject);
                        tmp.transform.SetParent(transform);
                        tmp.transform.localScale = new Vector3(1, 1, 1);
                        tmp.GetComponent<CanvasGroup>().blocksRaycasts = true;
                        tmp.GetComponent<MLADraggable>().beginDeck = this;
                    }
                    if (card.beginDeck.targetDeck == MLADecks.Furnace && targetDeck == MLADecks.Furnace) {
                        card.beginDeck = this;
                    }
                } else {
                    //Items restrictions
                    if (card.beginDeck.targetDeck == MLADecks.Result && (targetDeck == MLADecks.Deck || targetDeck == MLADecks.Belt || targetDeck == MLADecks.Furnace)) {
                        card.beginDeck = this;
                    }
                    if (card.beginDeck.targetDeck == MLADecks.Deck && (targetDeck == MLADecks.Belt || targetDeck == MLADecks.Furnace)) {
                        card.beginDeck = this;
                    }
                    if (card.beginDeck.targetDeck == MLADecks.Furnace && (targetDeck == MLADecks.Deck || targetDeck == MLADecks.Belt || targetDeck == MLADecks.Furnace)) {
                        card.beginDeck = this;
                    }
                    if (card.beginDeck.targetDeck == MLADecks.Belt && (targetDeck == MLADecks.Deck || targetDeck == MLADecks.Belt || targetDeck == MLADecks.Furnace)) {
                        card.beginDeck = this;
                    }
                }

            }

        }
    }
}
