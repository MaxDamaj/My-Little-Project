using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class MLADraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {

    public MLADeckDrop beginDeck;
    public bool interactable;

    private Transform Table;

    void Start() {
        Table = GameObject.Find("UI").transform;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!interactable) return;
        if (Table == null) Table = GameObject.Find("UI").transform;
        beginDeck = transform.parent.GetComponent<MLADeckDrop>();
        //Detach card
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(Table);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!interactable) return;
        //Moving card with pointer
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!interactable) return;
        //Attach card
        transform.SetParent(beginDeck.transform);
        transform.SetAsFirstSibling();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        beginDeck = transform.parent.GetComponent<MLADeckDrop>();
        UIItemsCraft.Instance.RefreshUI();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (beginDeck == null) return;
            if (!GetComponent<CraftComponent>().IsItem && beginDeck.targetDeck == MLADecks.Furnace) {
                transform.SetParent(Table);
                UIItemsCraft.Instance.RefreshUI();
                Destroy(gameObject);
            }
        }
    }
}
