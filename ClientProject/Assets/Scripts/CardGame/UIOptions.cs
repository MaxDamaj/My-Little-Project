using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIOptions : MonoBehaviour {

	public bool endlessOption;
	public GameObject endlessMark;
	public CardsSpawn tradeRowStyle;
	public Dropdown trStyle;
	public Image trImage;
	public Sprite[] trPicture;

	// Use this for initialization
	void Start () {
	
	}
	
	public void SetTradeRowStyle() {
		if (trStyle.value == 0) {
			tradeRowStyle = CardsSpawn.DivideRow;
			endlessOption = false;
			trImage.sprite = trPicture[0];
			endlessMark.SetActive(false);
		}
		if (trStyle.value == 1) {
			tradeRowStyle = CardsSpawn.DivideRow;
			endlessOption = true;
			trImage.sprite = trPicture[0];
			endlessMark.SetActive(true);
		}
		if (trStyle.value == 2) {
			tradeRowStyle = CardsSpawn.StandardRow;
			endlessOption = false;
			trImage.sprite = trPicture[1];
			endlessMark.SetActive(false);
		}
		if (trStyle.value == 3) {
			tradeRowStyle = CardsSpawn.StandardRow;
			endlessOption = true;
			trImage.sprite = trPicture[1];
			endlessMark.SetActive(true);
		}
		if (trStyle.value == 4) {
			tradeRowStyle = CardsSpawn.TypesRow;
			endlessOption = false;
			trImage.sprite = trPicture[2];
			endlessMark.SetActive(false);
		}
		if (trStyle.value == 5) {
			tradeRowStyle = CardsSpawn.TypesRow;
			endlessOption = true;
			trImage.sprite = trPicture[2];
			endlessMark.SetActive(true);
		}
	}
}
