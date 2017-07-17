using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICardGame : MonoBehaviour {

	[SerializeField]
	private DBChallenges DBC = null;

	[Header("Player")]
	public Transform playerDeck;
	public Transform playerStack;
	public Image playerIcon;
	public Text playerLUV;
	[Header("Enemy")]
	public Transform enemyDeck;
	public Image enemyIcon;
	public Text enemyLUV;
	[Header("UI")]
	public UIMessageWindow mWindow;
	public Button buttonOK;
	public Button buttonCancel;
	public Transform externalDeck;
	public Transform externalStack;

	private CardChallenge challenge;

	void Start() {
		buttonOK.onClick.AddListener(StartChallenge);
		gameObject.SetActive(false);
	}

	void OnEnable() {
		challenge = DBC.GetCardChallenge(GlobalData.Instance.nowChallenge);
		//Place new enemy cards
		for (int i = 0; i < challenge.enemyDeck.GetLength(0); i++) {
			GameObject tmp = Instantiate(Resources.Load<GameObject>("Cards/"+challenge.enemyDeck[i]));
			tmp.transform.SetParent(enemyDeck);
			tmp.transform.localScale = Vector3.one;
		}
		//Place new player cards
		for (int i = 0; i < externalDeck.childCount; i++) {
			GameObject tmp = Instantiate(externalDeck.GetChild(i).gameObject);
			tmp.transform.SetParent(playerDeck);
			tmp.transform.localScale = Vector3.one;
		}
		for (int i = 0; i < externalStack.childCount; i++) {
			GameObject tmp = Instantiate(externalStack.GetChild(i).gameObject);
			tmp.transform.SetParent(playerStack);
			tmp.transform.localScale = Vector3.one;
		}
		//-----
		playerIcon.sprite = Database.Instance.GetCharCardIcon(Database.Instance.SelectedPony);
		enemyIcon.sprite = challenge.enemyIcon;
		playerLUV.text = ""+challenge.playerLUV;
		enemyLUV.text = ""+challenge.enemyLUV;

	}

	void OnDisable() {
		//Remove old enemy cards
		int count = enemyDeck.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(enemyDeck.GetChild(i).gameObject);
		}
		//Remove old player cards
		count = playerDeck.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(playerDeck.GetChild(i).gameObject);
		}
		count = playerStack.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(playerStack.GetChild(i).gameObject);
		}
	}

	void StartChallenge() {
		if (playerDeck.childCount < 10) {
			mWindow.ShowMessage("You need " + 10 + " cards in your deck to begin challenge", 0, UIMessageWindow.Action.nothing, true, false);
		} else {
			ItemsController IC = GameObject.Find("UI_Items").GetComponent<ItemsController>();
			var challenge = DBC.GetChallenge(GlobalData.Instance.nowChallenge);
			foreach (var item in challenge.startFee) {
				Database.Instance.IncreaseItemQuantity(item.ItemName, -item.ItemQuantity);
			}
			while (playerDeck.childCount > 0) {
				playerDeck.GetChild(0).SetParent(IC.playerDeck);
			}
			while (enemyDeck.childCount > 0) {
				enemyDeck.GetChild(0).SetParent(IC.enemyDeck);
			}
			UnityEngine.SceneManagement.SceneManager.LoadScene("game_pvp");
		}
	}

}
