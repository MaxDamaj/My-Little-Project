using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UICraftTable : MonoBehaviour, IPointerDownHandler {

    public RectTransform craftGrid;
    public Image recipeSample;
    public Button clearButton;

    void Start() {
        for (int i = 0; i < Database.Instance.ArrayUsableItemsGetLenght(); i++) {
            if (Database.Instance.GetItemExist(Database.Instance.GetUsableItem(i).ItemName)) {
                if (Database.Instance.GetUsableItem(i).costItems.Count <= Database.Instance.furnaceSlots) {
                    GameObject tmp = Instantiate(recipeSample.gameObject);
                    tmp.transform.SetParent(craftGrid);
                    tmp.transform.localScale = Vector3.one;
                    tmp.name = Database.Instance.GetUsableItem(i).ItemName;
                    tmp.GetComponent<Image>().sprite = Database.Instance.GetItemIcon(tmp.name);
                }
            }
        }
        recipeSample.gameObject.SetActive(false);
        clearButton.onClick.AddListener(UIItemsCraft.Instance.ClearRecipeSamples);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (Database.Instance.GetItemExist(eventData.pointerEnter.name)) {
            var recipe = Database.Instance.GetUsableItem(eventData.pointerEnter.name);
            UIItemsCraft.Instance.SetRecipeItemsSalples(recipe.costItems.ToArray(), recipe.costPrices, recipe.ItemName);
            gameObject.SetActive(false);
        }
    }
}
