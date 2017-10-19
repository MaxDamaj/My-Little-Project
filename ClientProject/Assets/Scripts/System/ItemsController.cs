using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemsController : MonoBehaviour {

    [Header("Help Items")]
    public RectTransform itemWindow;
    public Transform inventory;
    public Transform[] belt;
    public Transform[] furnace;
    public UIFXComponent[] uifx;
    public Text infoText;

    private EndModeController _emc;

    private static ItemsController controller;

    #region API

    public static ItemsController Instance {
        get {
            if (controller == null) {
                controller = FindObjectOfType<ItemsController>();
            }
            return controller;
        }
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        HideItemWindow();
    }

    void Update() {
        //Keys Pressed
        if (itemWindow.anchoredPosition.y == 0) {
            if (_emc == null) _emc = FindObjectOfType<EndModeController>();
            /*//Left item (1)
            if (Input.GetAxis("D-Pad X") == -1) {
                ActivateItem(1);
            }
            //Center item (2)
            if (Input.GetAxis("D-Pad Y") == -1) {
                ActivateItem(2);
            }
            //Right item (3)
            if (Input.GetAxis("D-Pad X") == 1) {
                ActivateItem(3);
            }*/
        }
    }

    #endregion

    public void ActivateItem(int slot) {
        if (belt[slot-1].childCount > 0) {
            UseItem(belt[slot-1].GetComponentInChildren<CraftComponent>().title, slot);
            uifx[slot-1].ShowUIFX(3);
        }
    }

    public void HideItemWindow() {
        itemWindow.anchoredPosition = new Vector2(0, 200);
    }
    public void ShowItemWindow() {
        itemWindow.anchoredPosition = new Vector2(0, 0);
    }

    void UseItem(string itemName, int beltID) {
        if (_emc == null) return;
        switch (itemName) {
            case "Health Potion":
                GlobalData.Instance.currentHP += 10;
                break;
            case "Large Health Potion":
                GlobalData.Instance.currentHP += 25;
                break;
            case "Stamina Potion":
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 1000);
                break;
            case "Large Stamina Potion":
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 2500);
                break;
            case "Mana Potion":
                GlobalData.Instance.currentMP += 10;
                break;
            case "Large Mana Potion":
                GlobalData.Instance.currentMP += 25;
                break;
        }
        Destroy(belt[beltID].GetChild(0).gameObject);
    }
}
