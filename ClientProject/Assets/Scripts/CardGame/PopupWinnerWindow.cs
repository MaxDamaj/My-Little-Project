using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupWinnerWindow : MonoBehaviour {

	public PopupManager PM;
	public SceneManager SM;
	public DBChallenges DBC;

	[Header("Winner Popup")]
	public GameObject window;
	public Text winnerText;
	public Button winnerButton;

	private bool state;

	void Start() {
		window.SetActive(false);
		winnerButton.onClick.AddListener(ReturnToMenu);
	}

	public void Refresh() {

	}


	public void ShowPopup(bool winState) {
		state = winState;
		window.SetActive(true);
		if (state) {
			winnerText.text = "You Win! =)";
		} else {
			winnerText.text = "You Lose. =(";
		}
	}
		
	void ReturnToMenu() {
		if (state) {
			GainReward();
			Database.Instance.passedChallenges[GlobalData.Instance.nowChallenge]++;
		} 
		UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
	}
	void GainReward() {
		Challenge challenge = DBC.GetChallenge(GlobalData.Instance.nowChallenge);
		foreach (var item in challenge.reward) {
			Database.Instance.IncreaseItemQuantity(item.ItemName, item.ItemQuantity);
		}

	}

}
