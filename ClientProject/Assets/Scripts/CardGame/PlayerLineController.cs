using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLineController : MonoBehaviour {

	[Header("UI")]
	public Text textBIT;
	public Text textATK;
	public Text textLUV;
	public Text pDeckInfo;
	public Text playerText;
	public Image pileMask;
	public Image pImage;

	[Header("Decks")]
	public RectTransform pPile;
	public RectTransform pTurn;
	public RectTransform pDeck;
	public RectTransform pHand;
	public RectTransform pWarehouses;

	[Header("Common")]
	public int PlayerLUV = 50;

	// Use this for initialization
	void Start () {
		Refresh();
	}

	public void Refresh() {
		textLUV.text = "" + PlayerLUV;
	}
	

}
