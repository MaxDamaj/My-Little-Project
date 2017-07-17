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

    [Header("Cards Decks")]
    public Transform playerDeck;
    public Transform enemyDeck;

    private EndModeController _emc;

    void Start() {
        DontDestroyOnLoad(gameObject);
        itemWindow.anchoredPosition = new Vector2(0, 200);
    }

    void Update() {
        //Keys Pressed
        if (itemWindow.anchoredPosition.y == 0) {
            if (_emc == null) _emc = FindObjectOfType<EndModeController>();
            //Left item (1)
            if (Input.GetAxis("D-Pad X") == -1) {
                if (belt[0].childCount > 0) {
                    UseItem(belt[0].GetComponentInChildren<CraftComponent>().title, 0);
                    uifx[0].ShowUIFX(3);
                }
            }
            //Center item (2)
            if (Input.GetAxis("D-Pad Y") == -1) {
                if (belt[1].childCount > 0) {
                    UseItem(belt[1].GetComponentInChildren<CraftComponent>().title, 1);
                    uifx[1].ShowUIFX(3);
                }
            }
            //Right item (3)
            if (Input.GetAxis("D-Pad X") == 1) {
                if (belt[2].childCount > 0) {
                    UseItem(belt[2].GetComponentInChildren<CraftComponent>().title, 2);
                    uifx[2].ShowUIFX(3);
                }
            }
        }
        //Debug Info
        //infoText.text = "D-Pad X = " + Input.GetAxis("D-Pad X") + "  D-Pad Y = " + Input.GetAxis("D-Pad Y");
    }

    void UseItem(string itemName, int beltID) {
        if (_emc == null) return;
        switch (itemName) {
            case "Health Potion":
                _emc.currentHP += 10;
                break;
            case "Large Health Potion":
                _emc.currentHP += 25;
                break;
            case "Stamina Potion":
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 1000);
                break;
            case "Large Stamina Potion":
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 2500);
                break;
            case "Mana Potion":
                _emc.currentMP += 10;
                break;
            case "Large Mana Potion":
                _emc.currentMP += 25;
                break;
        }
        Destroy(belt[beltID].GetChild(0).gameObject);
    }
}
