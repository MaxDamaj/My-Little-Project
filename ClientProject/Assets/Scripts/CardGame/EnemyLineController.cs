using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EnemyLineController : MonoBehaviour, IPointerClickHandler {

	public SceneManager SM;

	public Color attackColor;
	public GameObject luvFrame;
	public Image pImage;
	public Text luvText;
	public Text enemyText;
	public RectTransform eWarehouses;
	public GameObject uifxHit;
	public AudioSource a_hit;

	public int LUV;

	void Start() {
		Refresh();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount == 2 && eventData.button == 0) {
			Card eCard = eventData.pointerEnter.GetComponent<Card>();
			if (eventData.pointerEnter == luvFrame) {
				AttackEnemy();
			}
			if (eCard != null) {
				AttackWarehouse(eCard);
			}
		}
	}

	public void Refresh() {
		luvText.text = "" + LUV;

		for (int i = 0; i < eWarehouses.childCount; i++) {
			Card card = eWarehouses.GetChild(i).GetComponent<Card>();
			if (SM.PlayerATK >= card.Protection) {
				if ((card.cardType == CardType.Warehouse && ReturnCastlesCount() == 0 && ReturnVillagesCount() == 0) ||
					(card.cardType == CardType.Castle && ReturnVillagesCount() == 0) ||
				    (card.cardType == CardType.Village)) {
					eWarehouses.GetChild(i).GetComponent<Image>().color = attackColor;
				}
			} else {
				eWarehouses.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
			}
		}
	}
		
	//-----------------------------------------------------
	public int ReturnCastlesCount() {
		int count = 0;
		for (int i = 0; i < eWarehouses.childCount; i++) {
			Card card = eWarehouses.GetChild(i).GetComponent<Card>();
			if (card.cardType == CardType.Castle) count++;
		}
		return count;
	}
	public int ReturnVillagesCount() {
		int count = 0;
		for (int i = 0; i < eWarehouses.childCount; i++) {
			Card card = eWarehouses.GetChild(i).GetComponent<Card>();
			if (card.cardType == CardType.Village) count++;
		}
		return count;
	}
	//-----------------------------------------------------
	public Card ReturnDestroyableWarehouse(int damage) {
		//List for all warehouses
		List<Card> warehouses = new List<Card>();
		for (int i=0; i<eWarehouses.childCount; i++) {
			warehouses.Add(eWarehouses.GetChild(i).GetComponent<Card>());
		}

		if (warehouses.Count == 0)
			return null;
		if (ReturnCastlesCount() > 0) {
			return warehouses.Find(x => (x.cardType == CardType.Castle && x.Protection <= damage));
		} else {
			return warehouses.Find(x => (x.cardType == CardType.Warehouse && x.Protection <= damage));
		}
	}
	//-----------------------------------------------------
	public void AttackEnemy() {
		if (SM.PlayerATK > 0 && ReturnCastlesCount() == 0) {
			LUV -= SM.PlayerATK;
			uifxHit.SetActive(true);
			a_hit.Play();
			SM.PlayerATK = 0;
			SM.RefreshUI();
			Refresh();
		}
	}
	public void AttackWarehouse(Card card) {
		if (SM.PlayerATK >= card.Protection) {
			if ((card.cardType == CardType.Warehouse && ReturnCastlesCount() == 0 && ReturnVillagesCount() == 0) ||
				(card.cardType == CardType.Castle && ReturnVillagesCount() == 0) ||
				(card.cardType == CardType.Village)) {
				SM.PlayerATK -= card.Protection;
				card.transform.Rotate(Vector3.forward, 90);
				card.transform.SetParent(SM.notNowPlayer.pPile);
				card.gameObject.GetComponent<Draggable>().enabled = true;
				card.GetComponent<Image>().color = new Color(1, 1, 1, 1);
				a_hit.Play();
				SM.RefreshUI();
			}
		}
	}
	//-----------------------------------------------------
}
