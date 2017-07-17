using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[Header("Main")]
	public Button buttonExit;
	[Header("PvP Player Stats")]
	public UIOptions options;
	public ButtonUniversal PvPBegin;
	public RectTransform p1Deck;
	public RectTransform p2Deck;
	public Text p1CardsText;
	public Text p2CardsText;
	public Image p1Icon;
	public Image p2Icon;
	public Text p1Name;
	public Text p2Name;
	public Text p1Luv;
	public Text p2Luv;
	[Header("Common")]
	public Sprite MMp1Icon;
	public Sprite MMp2Icon;
	public int MMp1LUV;
	public int MMp2LUV;
	public string MMp1Name;
	public string MMp2Name;

	public CardsSpawn tradeRowStyle;
	public bool endlessOption;
	public Image nowPlayerIcon;

	void Start () {
		DontDestroyOnLoad(gameObject);
		PvPBegin.onClick.AddListener(PvPBeginClick);
		buttonExit.onClick.AddListener(delegate {
			Application.Quit();	
		});
	}

	public void Refresh() {
		p1CardsText.text = p1Deck.childCount+"/10";
		p2CardsText.text = p2Deck.childCount+"/10";
		//Begin check state
		if (p1Deck.childCount == 10 && p2Deck.childCount == 10) {
			PvPBegin.SetState(ButtonState.Active);
		} else {
			PvPBegin.SetState(ButtonState.Locked);
		}
	}

	public void SetNowPlayerImage(Image img) {
		nowPlayerIcon = img;
	}

	void PvPBeginClick() {
		//Set trade row style
		tradeRowStyle = options.tradeRowStyle;
		endlessOption = options.endlessOption;
		//Save variables
		MMp1Icon = p1Icon.sprite;
		MMp2Icon = p2Icon.sprite;
		MMp1LUV = int.Parse(p1Luv.text);
		MMp2LUV = int.Parse(p2Luv.text);
		MMp1Name = p1Name.text;
		MMp2Name = p2Name.text;
		p1Deck.SetParent(transform);
		p2Deck.SetParent(transform);
		//--------------
		UnityEngine.SceneManagement.SceneManager.LoadScene("game_pvp");
	}
}
