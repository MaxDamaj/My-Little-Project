using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIMessageWindow : MonoBehaviour {

    public enum Action {
        nothing = 0,
        buying = 1,
        clear = 2,
        exit = 3,
        startChallenge = 4,
		restart = 5
    };

    [SerializeField]
    DatabaseManager DBM = null;
    [SerializeField]
    DBChallenges DBC = null;

    public Button ButtonYes;
    public Button ButtonNo;
    public Text questionText;

    private int ID;
    private Action action;

    // Use this for initialization
    void Start() {
        ButtonYes.onClick.AddListener(Confirm);
        ButtonNo.onClick.AddListener(Decline);
    }

    public void ShowMessage(string text, int id, Action mAction) {
        ID = id;
        action = mAction;
        ButtonYes.gameObject.SetActive(true);
        ButtonNo.gameObject.SetActive(true);
        questionText.text = text;
        gameObject.SetActive(true);
    }
    public void ShowMessage(string text, int id, Action mAction, bool bYes, bool bNo) {
        ID = id;
        action = mAction;
        ButtonYes.gameObject.SetActive(bYes);
        ButtonNo.gameObject.SetActive(bNo);
        questionText.text = text;
        gameObject.SetActive(true);
    }

    private void Confirm() {      
		switch (action) {
		//Buy pony
		case Action.buying: 
			Database.Instance.SetCharFMRank(ID, 0);
			CharsFMData pony = Database.Instance.GetCharFMInfo(ID);
			for (int i = 0; i < pony.costPrises.GetLength(0); i++) {
				Database.Instance.IncreaseItemQuantity(pony.costItems[i], 0 - pony.costPrises[i]);
			}
			break;
		//Clear State
		case Action.clear:
			DBM.ClearState();
			UnityEngine.SceneManagement.SceneManager.LoadScene("splashScreen");
			break;
		//Exit
		case Action.exit:
			Application.Quit();
			break;
		//Start Challenge
		case Action.startChallenge:
			var challenge = DBC.GetChallenge(ID);
			foreach (var item in challenge.startFee) {
				Database.Instance.IncreaseItemQuantity(item.ItemName, -item.ItemQuantity);
			}
			GlobalData.Instance.nowChallenge = ID;
			UnityEngine.SceneManagement.SceneManager.LoadScene("road_challenge");
			break;
		}
        gameObject.SetActive(false);
    }

    private void Decline() {
        gameObject.SetActive(false);
    }

}
