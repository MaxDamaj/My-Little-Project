using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPasswords : MonoBehaviour {

    public InputField PasswordField;

    void Start() {
        PasswordField.onEndEdit.AddListener(CheckPassword);
    }

    void CheckPassword(string password) {
        password = password.ToLower();
        switch (password) {
            //Materials
            case "trade ya!":
                Database.Instance.IncreaseItemQuantity("Bits", 1000);
                UIMessageWindow.Instance.ShowMessage("You gain 1000 Bits", 0, UIAction.nothing, true, false);
                break;
            case "feeling pinkie keen":
                Database.Instance.IncreaseItemQuantity("Laughter", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Laughter", 0, UIAction.nothing, true, false);
                break;
            case "suited for success":
                Database.Instance.IncreaseItemQuantity("Generosity", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Generosity", 0, UIAction.nothing, true, false);
                break;
            case "applebuck season":
                Database.Instance.IncreaseItemQuantity("Honesty", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Honesty", 0, UIAction.nothing, true, false);
                break;
            case "hurricane fluttershy":
                Database.Instance.IncreaseItemQuantity("Kindness", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Kindness", 0, UIAction.nothing, true, false);
                break;
            case "top bolt":
                Database.Instance.IncreaseItemQuantity("Loyalty", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Loyalty", 0, UIAction.nothing, true, false);
                break;
            case "the ticket master":
                Database.Instance.IncreaseItemQuantity("Magic", 100);
                UIMessageWindow.Instance.ShowMessage("You gain 100 Magic", 0, UIAction.nothing, true, false);
                break;
            case "fall weather friends":
                Database.Instance.IncreaseItemQuantity("Iron", 200);
                UIMessageWindow.Instance.ShowMessage("You gain 200 Iron", 0, UIAction.nothing, true, false);
                break;
            case "a friend in deed":
                Database.Instance.IncreaseItemQuantity("Copper", 200);
                UIMessageWindow.Instance.ShowMessage("You gain 200 Copper", 0, UIAction.nothing, true, false);
                break;
            case "sonic rainboom":
                Database.Instance.IncreaseItemQuantity("Tin", 200);
                UIMessageWindow.Instance.ShowMessage("You gain 200 Tin", 0, UIAction.nothing, true, false);
                break;
            //Specials
            case "":
                break;
            case "lesson zero":
                for (int i = 0; i < Database.Instance.ArrayItemsGetLenght(); i++) {
                    Database.Instance.SetItemQuantity(i, 0);
                }
                UIMessageWindow.Instance.ShowMessage("Too bad! You're lose all materials", 0, UIAction.nothing, true, false);
                break;
            //Incorrect password
            default:
                UIMessageWindow.Instance.ShowMessage("Incorrect password", 0, UIAction.nothing, true, false);
                break;
        }
    }
}
