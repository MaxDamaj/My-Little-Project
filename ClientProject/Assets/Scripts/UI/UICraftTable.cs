using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using MLA.System;

namespace MLA.UI.Windows {
    public class UICraftTable : MonoBehaviour, IPointerDownHandler {

        public RectTransform craftGrid;
        public Image recipeSample;
        public Button clearButton;

        #region API

        void Start() {
            SetRecipes();
            clearButton.onClick.AddListener(delegate {
                UIItemsCraft.Instance.ClearRecipeSamples();
                UIItemsCraft.Instance.RefreshUI();
            });
        }

        #endregion

        #region Events

        public void OnPointerDown(PointerEventData eventData) {
            if (Database.Instance.GetItemExist(eventData.pointerEnter.name)) {
                var recipe = Database.Instance.GetUsableItem(eventData.pointerEnter.name);
                UIItemsCraft.Instance.SetRecipeItemsSalples(recipe.costItems.ToArray(), recipe.costPrices, recipe.ItemName, recipe.exitQuantity);
                UIItemsCraft.Instance.RefreshUI();
                gameObject.SetActive(false);
            }
        }

        #endregion

        public void SetRecipes() {
            recipeSample.gameObject.SetActive(true);
            while (craftGrid.childCount > 0) {
                Destroy(craftGrid.GetChild(0));
            }
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
        }
    }
}
