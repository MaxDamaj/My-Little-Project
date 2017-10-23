using UnityEngine;
using System.Collections;

public class OnDestroyFX : MonoBehaviour {

    [SerializeField]
    private GameObject fx = null;
    [SerializeField]
    private float _objectDestroyDelay = 0;
    [SerializeField]
    private float _fxDestroyDelay = 3;

    public string objectType = "Bits";

    void Start() {
        PonyController.onPlayerPickup += ExecuteFX;
    }

    void ExecuteFX(string tag, GameObject target) {
        if (tag == "Player" && gameObject == target) {
            switch (objectType) {
                case "Bits":
                    SoundManager.Instance.PlaySound("a_coins");
                    PickupPopup.Instance.ShowPopupInfo("Bits");
                    Database.Instance.IncreaseItemQuantity("Bits", 1);
                    break;
                case "Laughter":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Laughter");
                    Database.Instance.IncreaseItemQuantity("Laughter", 1);
                    break;
                case "Generosity":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Generosity");
                    Database.Instance.IncreaseItemQuantity("Generosity", 1);
                    break;
                case "Honesty":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Honesty");
                    Database.Instance.IncreaseItemQuantity("Honesty", 1);
                    break;
                case "Kindness":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Kindness");
                    Database.Instance.IncreaseItemQuantity("Kindness", 1);
                    break;
                case "Loyalty":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Loyalty");
                    Database.Instance.IncreaseItemQuantity("Loyalty", 1);
                    break;
                case "Magic":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Magic");
                    Database.Instance.IncreaseItemQuantity("Magic", 1);
                    break;
                case "Iron":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Iron");
                    Database.Instance.IncreaseItemQuantity("Iron", 1);
                    break;
                case "Copper":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Copper");
                    Database.Instance.IncreaseItemQuantity("Copper", 1);
                    break;
                case "Tin":
                    SoundManager.Instance.PlaySound("a_beeps");
                    PickupPopup.Instance.ShowPopupInfo("Tin");
                    Database.Instance.IncreaseItemQuantity("Tin", 1);
                    break;
                case "Pixel":
                    SoundManager.Instance.PlaySound("a_coins");
                    DBSimulation.Instance.IncreaseItemQuantity("Pixel", 1);
                    break;
                case "Flow":
                    SoundManager.Instance.PlaySound("a_beeps");
                    DBSimulation.Instance.IncreaseItemQuantity("Flow", 1);
                    break;
            }
            Destroy(Instantiate(fx, transform.position, fx.transform.rotation), _fxDestroyDelay);
            Destroy(gameObject, _objectDestroyDelay);
        }
    }

    void OnDestroy() {
        PonyController.onPlayerPickup -= ExecuteFX;
    }
}
