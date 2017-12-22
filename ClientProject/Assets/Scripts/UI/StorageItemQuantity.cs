using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using MLA.System;

namespace MLA.UI.Common {
    public class StorageItemQuantity : MonoBehaviour {

        public static Color ACTIVE_COLOR = new Color(1, 1, 1, 1);
        public static Color UNACTIVE_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        public Image icon;
        public Text quantity;
        public MLADraggable item;

        private string _itemName = null;

        void Start() {
            Database.onRefresh += RefreshUI;
            _itemName = transform.parent.name;
            RefreshUI();
        }

        private void RefreshUI() {
            //Show quantity
            int count = Mathf.FloorToInt(Database.Instance.GetItemQuantity(_itemName));
            if (count <= 0) {
                quantity.text = "";
            }
            if (count > 0) {
                quantity.text = "" + count;
            }
            if (count >= 1000) {
                int v1 = Mathf.FloorToInt(count / 1000);
                int v2 = Mathf.FloorToInt(count / 100) - v1 * 10;
                quantity.text = "" + v1 + "." + v2 + "k";
            }
            if (count >= 1000000) {
                int v1 = Mathf.FloorToInt(count / 1e6f);
                int v2 = Mathf.FloorToInt(count / 1e5f) - v1 * 10;
                quantity.text = "" + v1 + "." + v2 + "M";
            }
            //Hide if inactive
            if (quantity.text == "") {
                icon.color = UNACTIVE_COLOR;
                quantity.color = UNACTIVE_COLOR;
                if (item != null) item.interactable = false;
            } else {
                icon.color = ACTIVE_COLOR;
                quantity.color = ACTIVE_COLOR;
                if (item != null) item.interactable = true;
            }
        }

        void OnDestroy() {
            Database.onRefresh -= RefreshUI;
        }
    }
}
