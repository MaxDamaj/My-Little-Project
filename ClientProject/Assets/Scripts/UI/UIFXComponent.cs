using UnityEngine;
using System.Collections;

namespace MLA.UI.Windows {
    public class UIFXComponent : MonoBehaviour {

        void Start() {
            //gameObject.SetActive(false);
        }

        public void ShowUIFX(float duration) {
            gameObject.SetActive(true);
            Invoke("HideUIFX", duration);
        }

        void HideUIFX() {
            gameObject.SetActive(false);
        }
    }
}