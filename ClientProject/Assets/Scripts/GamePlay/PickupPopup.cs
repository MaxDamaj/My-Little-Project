using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickupPopup : MonoBehaviour {

    public Image popupIcon;
    public Text popupText;
    public float disappearDelay = 1.0f;

    public static PickupPopup popup;



    public static PickupPopup Instance {
        get {
            if (popup == null) {
                popup = FindObjectOfType<PickupPopup>();
            }
            return popup;
        }
    }

    void Start() {
        popupIcon.gameObject.SetActive(false);
        popupText.gameObject.SetActive(false);
    }



    public void ShowPopupInfo(string item) {
        popupIcon.sprite = Database.Instance.GetItemIcon(item);
        popupText.text = "" + Mathf.FloorToInt(Database.Instance.GetItemQuantity(item));
        IEnumerator show = ShowPopup(disappearDelay);
        StopAllCoroutines(); StartCoroutine(show);
    }

    IEnumerator ShowPopup(float delay) {
        popupIcon.gameObject.SetActive(true);
        popupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        popupIcon.gameObject.SetActive(false);
        popupText.gameObject.SetActive(false);
    }
}
