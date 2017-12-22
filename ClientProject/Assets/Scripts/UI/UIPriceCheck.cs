using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MLA.System;

namespace MLA.UI.Windows {
    public class UIPriceCheck : MonoBehaviour {

        [SerializeField]
        private Color _colorGreen = Color.white;
        [SerializeField]
        private Color _colorRed = Color.white;

        public float value = 0;
        public string item = null;

        void Start() {
            Database.onRefresh += RefreshUI;
            RefreshUI();
        }

        void RefreshUI() {
            if (Database.Instance.GetItemQuantity(item) < value) {
                gameObject.GetComponent<Text>().color = _colorRed;
            } else {
                gameObject.GetComponent<Text>().color = _colorGreen;
            }
        }

        void OnDestroy() {
            Database.onRefresh -= RefreshUI;
        }
    }
}
